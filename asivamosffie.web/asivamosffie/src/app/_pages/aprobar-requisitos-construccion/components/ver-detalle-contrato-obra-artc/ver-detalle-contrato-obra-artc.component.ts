import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-ver-detalle-contrato-obra-artc',
  templateUrl: './ver-detalle-contrato-obra-artc.component.html',
  styleUrls: ['./ver-detalle-contrato-obra-artc.component.scss']
})
export class VerDetalleContratoObraArtcComponent implements OnInit {

  contrato: Contrato;

  constructor(
    private faseDosConstruccionSvc: FaseUnoConstruccionService,
    private activatedRoute: ActivatedRoute )
  {
    this.getContrato();
  }

  ngOnInit(): void {
  }

  getContrato() {
    this.faseDosConstruccionSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
      .subscribe(
        response => {
          this.contrato = response;
          console.log( this.contrato );
        }
      );
  }

}
