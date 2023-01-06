function pruebaPuntoNetStatic() {
    DotNet.invokeMethodAsync("BlazorPeliculas.Client", "ObtenerCurrentCount")
        .then(resultado => {
            console.log('conteo desde javascript ' + resultado);
        })
}

function pruebaPuntoNetInstancia(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount");
}

function timerInactivo(dotnetHelper) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        //timer = setTimeout(logout, 3*1000) // 3 segundos
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}