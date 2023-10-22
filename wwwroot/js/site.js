// Barra di ricerca
const searchbar = document.getElementById("searchbar");
const searchForm = document.getElementById("searchForm");
let timeoutId;
const delay = 1000;

if (searchbar && searchForm) {

searchbar.addEventListener("keyup", function () {
    clearTimeout(timeoutId); 

    timeoutId = setTimeout(function () {
        const searchQuery = searchbar.value.trim();
        if (searchQuery !== "") {
            localStorage.setItem("stringaRicerca", searchQuery);
            searchForm.submit();
        } else {
            if (window.location.href.includes("/Foto")) {
                window.location.href = "https://localhost:7001/Foto";
            } else {
                window.location.href = "https://localhost:7001";
            }
            localStorage.removeItem("stringaRicerca");
        }
    }, delay);

});

// Carica il valore memorizzato in localStorage nell'input al caricamento della pagina
searchbar.value = localStorage.getItem("stringaRicerca");
}

//------------------------------------------------------------------------------------------------------------------

//messaggio a scomparsa in INDEX per crud effettuata
const messageCrud = document.getElementById("messageCrud");

if (messageCrud) {
    messageCrud.classList.add("opa");
    setTimeout(() => {
        messageCrud.classList.remove("opa");
        messageCrud.classList.add("messageCrud");
    }, 1000)
}


//------------------------------------------------------------------------------------------------------------------


//preview immagine EDIT/CREATE crud
const input = document.getElementById("fileInput");
const img = document.getElementById("imgpreview");
if (input && img) previewImage();

function previewImage() {
    console.log("CIAO")
    input.addEventListener("change", function () {
        const selectedFile = this.files[0];
        const reader = new FileReader();

        reader.addEventListener("load", function () {
            img.src = reader.result;
        });

        if (selectedFile) {
            reader.readAsDataURL(selectedFile);
        }
    });
    
}

//------------------------------------------------------------------------------------------------------------------

//gestione modale eliminazione
const deleteBtn = document.querySelectorAll(".deleteBtn");
const modal = document.getElementById('exampleModalCenter');
const confermaEliminaBtn = document.getElementById('confermaEliminaBtn');
const close = document.getElementById('close');
const ics = document.getElementById('ics');
const modalInstance = new bootstrap.Modal(modal);
const form = document.getElementById('formDelete');

if (deleteBtn && form && modal) {


document.addEventListener("DOMContentLoaded", function () {
    const messageElement = document.getElementById("message");
    setTimeout(function () {
        messageElement.style.opacity = "0";
    }, 1000);
});

deleteBtn.forEach(function (btn) {
    btn.addEventListener('click', function (event) {
        event.preventDefault();
        modalInstance.show();
    });
});

ics.addEventListener('click', function () {
    modalInstance.hide();
});

close.addEventListener('click', function () {
    modalInstance.hide();
});

window.addEventListener('click', function (event) {
    modalInstance.hide();

});


confermaEliminaBtn.addEventListener('click', function () {
    //event.preventDefault();
    form.submit();

});
}
