//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    WinJS.UI.processAll().then(function () {

        ShowStatusPanel();
        GetCurrency();
        GraphCreate();
    });
})();

function ShowStatusPanel() {
    var panel = Windows.UI.ViewManagement.StatusBar.getForCurrentView();
    panel.showAsync();
    panel.backgroundOpacity = 0.99;
    panel.backgroundColor = Windows.UI.ColorHelper.fromArgb(255, 0x33, 0x7A, 0xB7); //#337ab7
    panel.foregroundColor = Windows.UI.Colors.lightGray;
}

function GetCurrency() {
    var ui = new UICore.Binding();
    var rate = ui.getTestString();
    var currencyElement = document.getElementById("currentCurrency");
    currencyElement.textContent = rate;
}

function createIndexLabel(element, points, currentIndex) {
    if (currentIndex == 0) {
        return '';
    }

    var firstElement = points[0].y;
    var prevElement = points[currentIndex - 1].y;
    var e = 0.003;
    var diff = Math.abs(element - prevElement);
    if (diff / firstElement < e) {
        return '';
    } else {
        return ((element - firstElement) / firstElement * 100).toFixed(2).toString(3) + '%';
    }
}

function GraphCreate(parameters) {
    var ui = new UICore.Binding();
    var rates = ui.getCurencesList();
    var dps = [];
    rates.forEach(function (element, index, array) {
        dps.push({
            x: new Date(2015, 3, index + 1),
            y: element,
            indexLabel: createIndexLabel(element, dps, index)
        });
    });
        var chart = new CanvasJS.Chart("chartContainer",
        {
            theme: "theme2",
            title:{
                text: "USD/BLR April:"
            },
            animationEnabled: true,
            animationDuration: 4000,
            axisX: {
                valueFormatString: "DD-MMM",
                interval:1,
                //intervalType: "day"
        
            },
            toolTip:{
                enabled: false
            },
            axisY: {
                includeZero: false,
                //interval: .25,
                //valueFormatString: "#.00'%'"
            },

            data: [
            {
                type: "area",
                toolTipContent: "{x}: {y}%",
                markerSize: 5,
                dataPoints: dps
            }

            ]
        });
        chart.render();
}