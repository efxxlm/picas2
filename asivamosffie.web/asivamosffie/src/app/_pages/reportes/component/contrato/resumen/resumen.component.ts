import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaContratoService } from 'src/app/core/_services/fichaContrato/ficha-contrato.service';

@Component({
  selector: 'app-resumen',
  templateUrl: './resumen.component.html',
  styleUrls: ['./resumen.component.scss']
})
export class ResumenComponent implements OnInit {
  pContratoId: string;
  openAcordeon = false;
  contrato: any;
  listProyectos: [];

  constructor(private route: ActivatedRoute, private fichaContratoService: FichaContratoService) {
    this.route.params.subscribe((params: Params) => (this.pContratoId = params.id));
  }

  ngOnInit(): void {
    this.getInfoResumenByContratoId();
  }

  getInfoResumenByContratoId() {
    this.fichaContratoService.getInfoResumenByContratoId(this.pContratoId).subscribe(response => {
      this.contrato = response[0].contrato;
      this.listProyectos = response[0].listProyectos;
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
