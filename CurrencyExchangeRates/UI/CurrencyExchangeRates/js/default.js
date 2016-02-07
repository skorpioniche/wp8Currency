//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    WinJS.UI.processAll().then(function () {
        ShowStatusPanel();
        LoadAllCyrrencies();
    });
})();

function LoadAllCyrrencies() {
    var currencyList = ['USD', 'EUR', 'RUB'];
    currencyList.forEach(function(element, index, array) {
        GetCurrency(element);
        GraphCreate(element);
    });
}

function ShowStatusPanel() {
    var panel = Windows.UI.ViewManagement.StatusBar.getForCurrentView();
    panel.showAsync();
    panel.backgroundOpacity = 0.99;
    panel.backgroundColor = Windows.UI.ColorHelper.fromArgb(0, 0, 0, 0x4);//fromArgb(255, 0x7d, 0xb9, 0xe8); //#7db9e8 
    panel.foregroundColor = Windows.UI.Colors.lightGray;
}

function UpdateCurrency(currencyName) {
    GetCurrency(currencyName);
    GraphCreate(currencyName);
}

function GetCurrency(currencyName) {
    var redColor = "#FF0000";
    var greenColor = "#00FF00";

    var ui = new UICore.Binding();
    var rate = ui.getLastCurrency(currencyName);
    var prevRate = ui.getCurrency(currencyName, rate.key);
    var diffRate = rate.value - prevRate.value;

    var currencyElement = document.querySelector("." + currencyName + " .currency-value");
    var currencyDayElement = document.querySelector("." + currencyName + " .currency-day"); 
    var currencyArrowElement = document.querySelector("." + currencyName + " .arrow");
    var currencyDiffElement = document.querySelector("." + currencyName + " .currency-diff");

    if (diffRate < 0) {
        currencyArrowElement.src = "images/downArrow.png";
        currencyDiffElement.style.color = redColor;
    } else if (diffRate > 0) {
        currencyDiffElement.style.color = greenColor;
        currencyArrowElement.src = "images/upArrow.png";
    } else {
        currencyDiffElement.style.display = "none";
        currencyArrowElement.style.display = "none"
    }

    currencyElement.textContent = Math.round(rate.value);
    currencyDayElement.textContent = rate.key;


    currencyDiffElement.textContent = diffRate.toFixed(1) + "(" + (Math.abs(diffRate) * 100 / prevRate.value).toFixed(2) + "%)"; //
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

function GraphCreate(currencyName) {
    var ui = new UICore.Binding();
    var rates = ui.getCurencesList(currencyName);
    var dps = [];
    rates.forEach(function (element, index, array) {
        dps.push({
            x: element.key,
            y: element.value,
            indexLabel: createIndexLabel(element.value, dps, index)
        });
    });
    var chart = new CanvasJS.Chart(document.querySelector("." + currencyName + " .currency-chart"),
            {
                
                backgroundColor: "#d6d6d6",
               
                    Axis: {
                        lineColor: "#FFF"
                    },
                theme: "theme4",
                title: {
                    text: currencyName + "/BYR:",
                    //backgroundColor: null
                },
                animationEnabled: true,
                animationDuration: 1500,
                axisX: {
                    valueFormatString: "DD-MMM",
                    interval: 5,
                    intervalType: "day"

                },
                toolTip: {
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
                xValueType: "dateTime",
                color: "red",
                //fillOpacity: 1,
                //markerSize: 5,
                dataPoints: dps
            }

            ]
        });
        chart.render();
}