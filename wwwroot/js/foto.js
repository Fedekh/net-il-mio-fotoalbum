﻿const searchBar = document.getElementById("search");
const row = document.getElementById("row");
const apiGet = `/api/foto/get`;
const apiDet = `/api/foto/getfoto/`;
const apiCategory = `/api/foto/getcategories`;
const apiMessage = `/Home/MessageView`;
let resultArray = [];
let totalPages;
let totalCount;
let currentPage = 1;
let categories = [];
let categoryChosen = [];

const spinner = document.querySelector(".spinner");
const section = document.querySelector(".section1");
const paginationDiv = document.getElementById("pagination");
const categoryDiv = document.getElementById("category");
const resetBtn = document.getElementById("reset")

resetBtn.addEventListener("click", () => {
    categoryChosen = [];
    // Deseleziona tutte le checkbox
    categories.forEach(category => {
        const input = document.getElementById(`${category.id}`);
        input.checked = false;
    });
    search();
})

getCategory()
function getCategory() {

    axios.get(apiCategory)
        .then((resp) => {
            categoryDiv.innerHTML = "";
            categories = resp.data;
            console.log(categories);

            categories.forEach(category => {
                categoryDiv.innerHTML += `
                <label for="${category.id}">${category.name}</label>
                <input type="checkbox" value="${category.name}" name="${category.name}" id="${category.id}"><br>
            `;
            });

            categories.forEach(category => {
                const input = document.getElementById(`${category.id}`);
                input.addEventListener('change', () => {
                    if (input.checked) {
                        categoryChosen.push(input.value);
                    } else {
                        const index = categoryChosen.indexOf(input.value);
                        if (index !== -1) {
                            categoryChosen.splice(index, 1);
                        }
                    }

                    console.log(categoryChosen)
                    search();
                });
            });

        })
        .catch((error) => {
            console.error('Errore durante il recupero delle categorie:', error);
        });
}


function search(search = "", page = 1) {
    if (!search) {
        spinner.classList.remove("d-none");
        section.classList.add("d-none");
    }

    let paramss = {
        search: search,
        catInput: categoryChosen.join(", "),
        pageNumber: page
    };

    axios.get(apiGet, { params: paramss })
        .then((resp) => {
            spinner.classList.add("d-none");
            section.classList.remove("d-none");
            resultArray = resp.data.fotos;
            currentPage = resp.data.pageNumber; // Pagina attuale
            pageSize = resp.data.pageSize;  // Risultati per pagina
            totalPages = resp.data.totalPages; // Totale pagine
            totalCount = resp.data.totalCount; //risultati totali

            row.innerHTML = "";
            if (resultArray.length === 0) {
                row.innerHTML = `<p>Nessun risultato</p>`;
            }
            else {
                document.getElementById("founded").innerHTML =
                    `<p class="text-warning">Trovati: ${totalCount} risultati totali </p> <p class="text-info">
                        Pagina: ${currentPage} <br> Risultati : ${resultArray.length}</p>
                        `;

                resultArray.forEach(elem => {
                    if (elem.isVisible) {
                        row.innerHTML += `
                                <div class="col">
                                    <div class="card h-100">
                                        <img src="${src(elem.imageFile)}" class="card-img-top">
                                        <div class="card-body">
                                            <h5 class="card-title">TITOLO: ${elem.name}</h5>
                                            <p class="card-text">DESCRIZIONE: ${elem.description}</p>
                                            <div class="card-footer">
                                                <a class="btn btn-primary det" det-id="${elem.id}">Dettagli</a>
                                                <button type="button" class="btn btn-outline-dark btn-floating message m-1" message-id="${elem.ownerName}">
                                                    <i class="fa-regular fa-envelope fa-bounce"></i> Contatta l'autore
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            `;
                    }
                });

                const messageButtons = document.querySelectorAll(".message");
                messageButtons.forEach(button => {
                    button.addEventListener("click", function () {
                        const ownerName = button.getAttribute("message-id");

                        axios.get(`${apiMessage}`, {
                            params: {
                                ownerName: ownerName,
                            }
                        })
                            .then((response) => {
                                console.log(response);
                                window.location.href = `/Home/MessageView?ownerName=${ownerName}`;
                            })
                            .catch((error) => {
                                console.error(error);
                            });
                    });
                });

                const detButtons = document.querySelectorAll(".det");
                detButtons.forEach(button => {
                    button.addEventListener("click", function () {
                        const id = this.getAttribute("det-id");
                        axios.get(`${apiDet}${id}`)
                            .then((resp) => {
                                const detArray = resp.data;
                                console.log(detArray);

                                const categories = detArray.categories.length > 0 ? detArray.categories.map(category => category.name).join(", ")
                                    : "<p>Nessuna categoria associata.</p>";

                                const detailsContent = document.getElementById("detailsContent");

                                detailsContent.innerHTML = `
                                        <div>
                                            <h3>Dettagli:</h3>
                                            <p>Autore: ${detArray.ownerName}</p>
                                            <p>ID: ${detArray.id}</p>
                                            <p>Nome: ${detArray.name}</p>
                                            <p>Descrizione: ${detArray.description}</p>
                                            <div>
                                                <p>Categorie:</p>
                                                ${categories}
                                            </div>
                                        </div>
                                        <div>
                                            <img width="700" src="${src(detArray.imageFile)}" class="card-img-top imgdet">
                                        </div>
                                    `;

                                function hideDetailsModal() {
                                    const detailsModal = document.getElementById("detailsModal");
                                    detailsModal.style.display = "none";
                                }

                                // Mostra il div dei dettagli
                                const detailsModal = document.getElementById("detailsModal");
                                detailsModal.style.display = "block";

                                const closeModal = document.getElementById("closeModal");
                                closeModal.addEventListener("click", function () {
                                    hideDetailsModal();
                                });

                                // chiudere il modale cliccando ovunque
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

            // Aggiorna la paginazione
            generatePagination(totalPages, currentPage);
        });
}

// paginazione
function generatePagination(totalPages, currentPage) {
    paginationDiv.innerHTML = "";
    const prevButton = document.createElement("a");

    prevButton.classList.add("btn", "btn-secondary");
    prevButton.textContent = "Precedente";
    prevButton.addEventListener("click", () => {
        if (currentPage > 1) {
            search(searchBar.value, currentPage - 1);
        }
    });

    paginationDiv.append(prevButton);

    // Pulsanti pagine
    for (let i = 1; i <= totalPages; i++) {
        const pageButton = document.createElement("a");
        pageButton.classList.add("btn", "btn-secondary", "page-number");
        pageButton.textContent = i;
        pageButton.addEventListener("click", () => {
            search(searchBar.value, i);
        });
        if (i === currentPage) {
            pageButton.classList.add("active");
            pageButton.disabled = true;
        }
        paginationDiv.append(pageButton);
    }

    const nextButton = document.createElement("a");
    nextButton.classList.add("btn", "btn-secondary");
    nextButton.textContent = "Successivo";
    nextButton.addEventListener("click", () => {
        if (currentPage < totalPages) {
            search(searchBar.value, currentPage + 1);
        }
    });
    paginationDiv.append(nextButton);
}

// Inizializza la pagina
search();

searchBar.addEventListener("input", function () {
    const searchValue = searchBar.value;
    search(searchValue);
});

function src(img) {
    return img ? `data:image/png;base64, ${img}` : "https://i1.sndcdn.com/avatars-CBHm1GPyWVXctJAz-B0ySmg-t240x240.jpg";
}
