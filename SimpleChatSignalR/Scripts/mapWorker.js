onmessage = function (e) {
    var index = 1;
    var succes = true;
    while (succes) {
        if (e.data[index].lat == e.data[0].lat) {
            if (e.data[index].lng == e.data[0].lng) {
                Vrati(e.data[index]);
                succes = false;
            }
        }
        index++;
        if (index == e.data.length) {
            Vrati("promasaj");
            succes = false;
        }
    }
}

function Vrati(data) {
    postMessage(data);
}
