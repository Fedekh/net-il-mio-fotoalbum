const apiMessage = "/api/message/CreateMessage";
const emailInput = document.getElementById("email");
const textArea = document.getElementById("text-area");
const notify = document.getElementById("notify");
const btnMessage = document.getElementById("btn-message");

btnMessage.addEventListener("click", function () {
    event.preventDefault(); // Correggi l'errore di battitura qui
    sendMessage();
});

function sendMessage() {
    // Otteniamo i dati dall'input dell'utente
    const email = emailInput.value;
    const text = textArea.value;

    // Creiamo un oggetto con i dati del messaggio
    const message = {
        email: email,
        text: text
    };

    // Effettuiamo una richiesta POST per inviare il messaggio al server utilizzando Axios
    axios.post(apiMessage, message)
        .then(response => {
            notify.innerHTML = "Messaggio inviato con successo!";
            notify.classList.remove("d-none");

            // Aggiungi un ritardo di 5 secondi prima di nascondere il messaggio di notifica
            setTimeout(() => {
                notify.innerHTML = "";
                notify.classList.add("d-none");
            }, 5000);

            // Reset dei campi dell'input
            emailInput.value = "";
            textArea.value = "";
        })
        .catch(error => {
            // Se c'è un errore, mostriamo un messaggio di errore
            console.error(error);
            notify.innerHTML = "Si è verificato un errore durante l'invio del messaggio.";
            notify.classList.remove("d-none");
        });
}
