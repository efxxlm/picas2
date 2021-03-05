import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-ver-detalle-perfiles',
  templateUrl: './ver-detalle-perfiles.component.html',
  styleUrls: ['./ver-detalle-perfiles.component.scss']
})
export class VerDetallePerfilesComponent implements OnInit {

  contrato: Contrato;

  constructor ( private activatedRoute: ActivatedRoute,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) 
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId ( pContratoId: string ) {
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( contrato => {
        this.contrato = contrato;
        console.log( this.contrato );
      } );
  };

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml;
  };

}
