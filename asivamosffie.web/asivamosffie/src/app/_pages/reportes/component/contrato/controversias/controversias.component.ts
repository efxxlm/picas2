import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaContratoService } from 'src/app/core/_services/fichaContrato/ficha-contrato.service';

@Component({
  selector: 'app-controversias',
  templateUrl: './controversias.component.html',
  styleUrls: ['./controversias.component.scss']
})
export class ControversiasComponent implements OnInit {
  pContratoId: string;
  openAcordeon = false;
  contrato: any;

  constructor(private route: ActivatedRoute, private fichaContratoService: FichaContratoService) {
    this.route.params.subscribe((params: Params) => (this.pContratoId = params.id));
  }

  ngOnInit(): void {
    this.getInfoControversiasByContratoId();
  }

  getInfoControversiasByContratoId() {
    this.fichaContratoService
      .getInfoControversiasByContratoId(this.pContratoId)
      .subscribe(response => (this.contrato = response));
  }

  downloadPDF() {
    this.openAcordeon = true;
    setTimeout(() => {
      document.title = 'Controversias ' + this.contrato.numeroContrato;
      window.print();
    }, 300);
    setTimeout(() => (this.openAcordeon = true), 400);
    // window.onafterprint = function () {
    //   window.location.reload();
    // };
  }
}
