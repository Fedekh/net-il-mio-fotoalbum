const searchBar = document.getElementById("search");
const row = document.getElementById("row");
const apiGet = `api/foto/get`;
const apiDet = `api/foto/getfoto/`;
const resultArray = [];


function src(img) {
    return img ? `data:image/png;base64, ${img}` : "https://i1.sndcdn.com/avatars-CBHm1GPyWVXctJAz-B0ySmg-t240x240.jpg";
 
}

function search(search) {
    axios.get(apiGet, {
        params: {
            search: search
        }
    })
        .then((resp) => {
            const resultArray = resp.data;
            console.log(resultArray);
            row.innerHTML = "";
            if (resultArray.length === 0) {
                `<p>Nessun risultato</p>`;
            }
            else {

                resultArray.forEach(elem => {
                    row.innerHTML += `
                    <div class="col">
                        <div class="card h-100">
                            <img src="${src(elem.imageFile)}" class="card-img-top">
                            <div class="card-body">
                                <h5 class="card-title">TITOLO: ${elem.name}</h5>
                                <p class="card-text">DESCRIZIONE: ${elem.description}</p>
                                <div class="card-footer">
                                    <a class="btn btn-primary det" data-id="${elem.id}">Dettagli</a>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                });

                // Aggiungi un event listener ai pulsanti "Dettagli"
                const detButtons = document.querySelectorAll(".det");
                detButtons.forEach(button => {
                    button.addEventListener("click", function () {
                        const id = this.getAttribute("data-id");
                        axios.get(`${apiDet}${id}`)
                            .then((resp) => {
                                const detArray = resp.data;

                                const categories = detArray.categories.length > 0 ? detArray.categories.map(category => category.name).join(", ")
                                    : "<p>Nessuna categoria associata.</p>";

                                const detailsContent = document.getElementById("detailsContent");

                                detailsContent.innerHTML = `
                                                            <h2>Dettagli:</h2>
                                                            <img src="${src(detArray.imageFile)}" class="card-img-top imgdet">
                                                            <p>ID: ${detArray.id}</p>
                                                            <p>Nome: ${detArray.name}</p>
                                                            <p>Descrizione: ${detArray.description}</p>
                                                           <div>
                                                                <h3>Categorie:</h3>
                                                                ${categories}
                                                           </div>
                                                        `;

                                function hideDetailsModal() {
                                    const detailsModal = document.getElementById("detailsModal");
                                    detailsModal.style.display = "none";
                                }


                                // Mostra il div dei dettagli
                                const detailsModal = document.getElementById("detailsModal");
                                detailsModal.style.display = "block";

                                // Chiudi il div dei dettagli quando si fa clic sulla "X" (close button)
                                const closeModal = document.getElementById("closeModal");
                                closeModal.addEventListener("click", function () {
                                    hideDetailsModal();
                                });


                                // Aggiungi un event listener alla finestra per chiudere il modale cliccando ovunque al di fuori
                                window.addEventListener("click", function (event) {
                                    const detailsModal = document.getElementById("detailsModal");
                                    const section = document.querySelector("section");
                                    if (event.target === detailsModal || event.target === section) {
                                        hideDetailsModal();
                                    }
                                });

                            })
                            .catch((error) => {
                                console.error(error);
                            });
                    });
                });
            }
        });
}


document.addEventListener("DOMContentLoaded", function () {   

    search();
    searchBar.addEventListener("input", function () {
        const searchValue = searchBar.value;
        search(searchValue);
    });

    
});
