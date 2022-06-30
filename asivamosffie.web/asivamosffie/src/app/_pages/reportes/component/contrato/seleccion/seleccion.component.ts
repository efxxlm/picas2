import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaContratoService } from 'src/app/core/_services/fichaContrato/ficha-contrato.service';

@Component({
  selector: 'app-seleccion',
  templateUrl: './seleccion.component.html',
  styleUrls: ['./seleccion.component.scss']
})
export class SeleccionComponent implements OnInit {
  pContratoId: string;
  openAcordeon = false;
  contrato: any;

  constructor(private route: ActivatedRoute, private fichaContratoService: FichaContratoService) {
    this.route.params.subscribe((params: Params) => (this.pContratoId = params.id));
  }

  ngOnInit(): void {
    this.getInfoProcesosSeleccionByContratoId();
  }

  getInfoProcesosSeleccionByContratoId() {
    this.fichaContratoService.getInfoProcesosSeleccionByContratoId(this.pContratoId).subscribe(response => {
      this.contrato = response;
    });
  }

  downloadPDF() {
    this.openAcordeon = true;
    setTimeout(() => {
      document.title = 'Selección ' + this.contrato.numeroContrato;
      window.print();
    }, 300);
    setTimeout(() => (this.openAcordeon = true), 400);
    // window.onafterprint = function () {
    //   window.location.reload();
    // };
  }
}
