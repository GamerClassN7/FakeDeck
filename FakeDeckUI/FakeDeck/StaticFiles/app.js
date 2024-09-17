
function urlencodeFormData(fd){
    var s = '';
    function encode(s){ return encodeURIComponent(s).replace(/%20/g,'+'); }
    for(var pair of fd.entries()){
        if(typeof pair[1]=='string'){
            s += (s?'&':'') + encode(pair[0])+'='+encode(pair[1]);
        }
    }
    return s;
}

function resizeGrid(){
    var width = document.body.clientWidth;
    var height = document.body.clientHeight;
    width = window.screen.availWidth;
    height = window.screen.availHeight;

    
    var biggerSizeW = (width / 7);
    var biggerSizeH = (height / 5);

    var biggerSize = biggerSizeW;
    if(biggerSizeW < biggerSizeH){
        biggerSize = biggerSizeW;
    }

    console.log(biggerSize);

    [].forEach.call(document.querySelectorAll('.button'), function (button) {
        var size = biggerSize
        button.style.height = size;
        button.style.width = size;
        //console.log(size);
    });
}

function loadPage(pageId){
    event.preventDefault();
    
    var formData = new FormData();
    formData.append('Key', pageId)

    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () { }
    xhttp.onreadystatechange = function () {
        if (xhttp.readyState === 4) {
            if (xhttp.status === 200) {
                console.log('successful');
                document.querySelector('div#main').innerHTML = xhttp.responseText;
                console.log(xhttp.responseText);
                resizeGrid();
                formToAjax();
            } else {
                console.log('failed');
            }
        }
    }

    xhttp.open("POST", "/page", true);
    xhttp.send(urlencodeFormData(formData));
}

function formToAjax(){
    [].forEach.call(document.querySelectorAll('form'), function (form) {
        form.addEventListener('submit', function (event) {
            event.preventDefault();
            const target = event.currentTarget;
            target.style.opacity = "0.5"

            console.log(target.method, target.action);
            console.log(form.method, form.action);
            console.log(form === target);

            var formData = new FormData(form);

            const xhttp = new XMLHttpRequest();
            xhttp.onload = function () { }
            xhttp.onreadystatechange = function () {
                if (xhttp.readyState === 4) {
                    if (xhttp.status === 200) {
                        console.log('successful');
                    } else {
                        console.log('failed');
                    }
                    target.style.opacity = "1"
                }
            }

            xhttp.open(target.method, target.action, true);
            xhttp.send(urlencodeFormData(formData));
        });
    });
}

window.addEventListener('resize', function(event) {
     resizeGrid();
}, true);

window.addEventListener('orientationchange', function(event) {
    console.log("Rotated");
    resizeGrid();
}, true);

 resizeGrid();
 formToAjax();