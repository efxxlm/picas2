import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaContratoService } from 'src/app/core/_services/fichaContrato/ficha-contrato.service';


@Component({
  selector: 'app-polizas-seguros',
  templateUrl: './polizas-seguros.component.html',
  styleUrls: ['./polizas-seguros.component.scss']
})
export class PolizasSegurosComponent implements OnInit {
  listaActualizacion = [
    'Buen manejo y correcta inversión del anticipo',
    'Garantía de estabilidad y calidad de la obra'
  ]

  pContratoId: string;
  openAcordeon = false;
  contrato: any;
  listProyectos: [];

  constructor(private route: ActivatedRoute, private fichaContratoService: FichaContratoService) {
    this.route.params.subscribe((params: Params) => (this.pContratoId = params.id));
  }

  ngOnInit(): void {
    this.getInfoPolizasSegurosByContratoId();
  }

  getInfoPolizasSegurosByContratoId() {
    this.fichaContratoService.getInfoPolizasSegurosByContratoId(this.pContratoId).subscribe(response => {
      this.contrato = response;
    });
  }

  downloadPDF() {
    this.openAcordeon = true;
    setTimeout(() => {
      document.title = 'Resumen ' + this.contrato.numeroContrato;
      window.print();
    }, 300);
    setTimeout(() => (this.openAcordeon = true), 400);
    // window.onafterprint = function () {
    //   window.location.reload();
    // };
  }
}
