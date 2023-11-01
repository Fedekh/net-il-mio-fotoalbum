const urlFoto = window.location.href;

if (urlFoto.includes("/Details")) {
    const initImg = document.querySelector('.imgInit');
    const zoomImg = document.querySelector('.imgZoom');

    if (initImg && zoomImg) {

        initImg.addEventListener('click', function () {
            zoomImg.classList.remove("d-none");
        });

        zoomImg.addEventListener("click", function () {
            zoomImg.classList.add("d-none");
        })
    }
} 

if (urlFoto.includes("/Foto")) {
    const img = document.querySelectorAll('.imgAdmin');
    img.forEach(e => {
        e.addEventListener('click', (f) => {
            f.stopPropagation();
            const imgZoom = e.nextElementSibling;
            imgZoom.classList.toggle('d-none');

            imgZoom.addEventListener('click', function (k) {
                k.stopPropagation();
                imgZoom.classList.add('d-none');
            })

            document.addEventListener('click', function (event) {
                event.stopPropagation();

                imgZoom.classList.add('d-none');
            });
        });
    });


}

