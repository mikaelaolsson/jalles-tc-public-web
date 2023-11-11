"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const ExcelJS = require("exceljs");
// Function to convert Excel data to HTML table (same as previous code)
function excelToHtmlTable(data) {
    const table = document.createElement('table');
    table.classList.add('excel-table');
    data.forEach((row) => {
        const tr = document.createElement('tr');
        row.forEach((cellData) => {
            const td = document.createElement('td');
            td.textContent = cellData;
            tr.appendChild(td);
        });
        table.appendChild(tr);
    });
    return table;
}
// Function to read Excel file and display as HTML table
function displayExcelAsHtmlTable(file) {
    return __awaiter(this, void 0, void 0, function* () {
        const workbook = new ExcelJS.Workbook();
        const arrayBuffer = yield file.arrayBuffer(); // Read file contents as ArrayBuffer
        yield workbook.xlsx.load(arrayBuffer);
        const sheet = workbook.worksheets[0]; // Assuming you want to use the first sheet
        const excelData = [];
        sheet.eachRow((row) => {
            const rowData = [];
            row.eachCell((cell) => {
                rowData.push(cell.value);
            });
            excelData.push(rowData);
        });
        const table = excelToHtmlTable(excelData);
        const container = document.getElementById('excel-container');
        if (container) {
            container.innerHTML = '';
            container.appendChild(table);
        }
    });
}
// Example usage (file input element is used for user interaction)
const fileInput = document.getElementById('file-input');
fileInput.addEventListener('change', (e) => {
    var _a;
    const file = (_a = e.target.files) === null || _a === void 0 ? void 0 : _a[0];
    if (file) {
        displayExcelAsHtmlTable(file);
    }
});
//# sourceMappingURL=excel-to-html.js.map