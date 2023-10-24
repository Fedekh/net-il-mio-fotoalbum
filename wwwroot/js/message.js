const apiMessage = "/api/message/CreateMessage";
const emailInput = document.getElementById("email");
const textArea = document.getElementById("text-area");
const notify = document.getElementById("notify");
const btnMessage = document.getElementById("btn-message");

btnMessage.addEventListener("click", function () {
    event.preventDefault(); 
    sendMessage();
});

function sendMessage() {
    const email = emailInput.value;
    const text = textArea.value;

    const message = {
        email: email,
        text: text
    };

    axios.post(apiMessage, message)
        .then(response => {
            notify.innerHTML = "Messaggio inviato con successo!";
            notify.classList.remove("d-none");

            setTimeout(() => {
                notify.innerHTML = "";
                notify.classList.add("d-none");
            }, 5000);

            emailInput.value = "";
            textArea.value = "";
        })
        .catch(error => {
            console.error(error);
            notify.innerHTML = "Si è verificato un errore durante l'invio del messaggio.";
            notify.classList.remove("d-none");
        });
}
