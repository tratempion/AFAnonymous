var url = 'localhost:7071/api/getDecaissement';

    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    if (!xhr) {
		console.log('CORS not suported');
    }
    xhr.onload = function () {
		console.log(xhr.responseText);
    };

    xhr.onerror = function (retour) {
      console.log('erreur : ' + retour);
    };

    xhr.send();