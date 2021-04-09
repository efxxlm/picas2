export class FileDownloader{

    public static exportExcel(fileName: string, respuesta){
        let documento = `${fileName}.xlsx`;
        var text = documento,
          blob = new Blob([respuesta], { type: 'application/octet-stream' }),
          anchor = document.createElement('a');
        anchor.download = fileName;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
    }

    public static exportPDF(fileName: string, respuesta){
      let documento = `${fileName}.pdf`;
      var text = documento,
        blob = new Blob([respuesta], { type: 'application/pdf' }),
        anchor = document.createElement('a');
      anchor.download = fileName;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
  }
}