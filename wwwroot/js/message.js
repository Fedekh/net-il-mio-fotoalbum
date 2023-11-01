const apiMessage = "/api/message/CreateMessage";
const emailInput = document.getElementById("email");
const textInput = document.getElementById("text-area");
const btnMessage = document.getElementById("btn-message");
const notify = document.getElementById("notify");
const emailError = document.querySelector("#email + .text-danger");
const textError = document.querySelector("#text-area + .text-danger");

const url = window.location.href;
const urlObject = new URL(url);
const autore = urlObject.searchParams.get("ownerName");

btnMessage.addEventListener("click", function () {
    const emailValue = emailInput.value.trim();
    const textValue = textInput.value.trim();

    // Verifica se i campi sono vuoti e mostra gli span di errore se necessario
    if (emailValue === "") {
        emailError.classList.remove("d-none");
    } else {
        emailError.classList.add("d-none");
    }

    if (textValue === "") {
        textError.classList.remove("d-none");
    } else {
        textError.classList.add("d-none");
    }

    // Se entrambi i campi sono compilati, invia il messaggio
    if (emailValue !== "" && textValue !== "") {
        sendMessage(autore);
    }
});

// Aggiungi gestori di eventi input per email e textInput per nascondere gli span di errore
emailInput.addEventListener("input", function () {
    if (emailInput.value.trim() !== "") {
        emailError.classList.add("d-none");
    }
});

textInput.addEventListener("input", function () {
    if (textInput.value.trim() !== "") {
        textError.classList.add("d-none");
    }
});

function sendMessage(ownerName) {
    const newEmail = emailInput.value.trim();
    const newText = textInput.value.trim();

    const message = {
        email: newEmail,
        text: newText,
        ownerName: ownerName,
    };

    axios
        .post(apiMessage, message)
        .then((response) => {
            notify.innerHTML = "Messaggio inviato con successo!";
            notify.classList.remove("d-none");
            notify.classList.remove("text-danger");
            notify.classList.add("text-success");


            setTimeout(() => {
                notify.innerHTML = "";
                notify.classList.add("d-none");
            }, 5000);

            emailInput.value = "";
            textInput.value = "";
        })
        .catch((error) => {
            console.error(error);
            notify.innerHTML = "Si è verificato un errore durante l'invio del messaggio.\n Probabilmente il formato dell'email";
            notify.classList.remove("d-none");
            notify.classList.add("text-danger");

        });
}
