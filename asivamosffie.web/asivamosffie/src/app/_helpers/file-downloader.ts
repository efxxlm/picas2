export class FileDownloader{

    public static exportExcel(fileName: string, respuesta){
        let documento = "ReportePagos.xlsx";
        var text = documento,
          blob = new Blob([respuesta], { type: 'application/octet-stream' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
    }
}