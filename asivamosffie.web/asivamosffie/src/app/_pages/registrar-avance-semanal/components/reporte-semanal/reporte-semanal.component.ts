import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';

@Component({
  selector: 'app-reporte-semanal',
  templateUrl: './reporte-semanal.component.html',
  styleUrls: ['./reporte-semanal.component.scss']
})
export class ReporteSemanalComponent implements OnInit {

  contratacionProyectoId: string;
  seguimientoSemanalId: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private commonSvc: CommonService,
    private registrarAvanceSemanalService: RegistrarAvanceSemanalService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.contratacionProyectoId;
      this.seguimientoSemanalId = params.seguimientoSemanalId;
      console.log(this.contratacionProyectoId);
      console.log(this.seguimientoSemanalId);
      this.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId(this.contratacionProyectoId, this.seguimientoSemanalId)

    });
  }

  getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( pContratacionProyectoId, pSeguimientoSemanalId ) {
    this.registrarAvanceSemanalService.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId(pContratacionProyectoId, pSeguimientoSemanalId).subscribe(
      response => {
        console.log(response)
    })
  }

  downloadPDF() {
    // html2canvas( document.getElementById( 'example-pdf' ) ).then(canvas => {
    //   const contentDataURL = canvas.toDataURL('image/jpeg', 1.0)

    //   let pdf = new jsPDF('l', 'cm', 'a4', false); //Generates PDF in landscape mode
    //   // let pdf = new jsPDF('p', 'cm', 'a4'); //Generates PDF in portrait mode
    //   pdf.addImage(contentDataURL, 'PNG', 0, 0, 29.8, 21.5, 'undefined', 'FAST')
    //   pdf.save('Filename.pdf');
    // });
    console.log(document.getElementById('example-pdf').innerHTML);
    const pdfHTML = document.getElementById('example-pdf').innerHTML;

    const pdf = {
      EsHorizontal: true,
      MargenArriba: 2,
      MargenAbajo: 2,
      MargenDerecha: 2,
      MargenIzquierda: 2,
      Contenido: pdfHTML
    };

    this.commonSvc.GetHtmlToPdf(pdf).subscribe(
      response => {
        const documento = `OrdernGiro.pdf`;
        const text = documento,
          blob = new Blob([response], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
      },
      e => {
        console.log(e);
      }
    );
  }

}
