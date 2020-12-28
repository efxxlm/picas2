import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { CommonService } from '../../../../core/_services/common/common.service';

@Component({
  selector: 'app-ver-detalle',
  templateUrl: './ver-detalle.component.html',
  styleUrls: ['./ver-detalle.component.scss']
})
export class VerDetalleComponent implements OnInit {

  contrato: Contrato;
  perfilesCv: Dominio[] = [];

  constructor ( private activatedRoute: ActivatedRoute,
                private commonSvc: CommonService,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) 
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
    this.commonSvc.listaPerfil()
      .subscribe(
        response => this.perfilesCv = response
      );
  };

  ngOnInit(): void {
  };

  getContratacionByContratoId ( pContratoId: string ) {
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( contrato => {
        this.contrato = contrato;
        console.log( this.contrato );
      } );
  };

  tipoPerfil ( perfilCodigo: string ) {
    let perfilSeleccionado;
    if ( this.perfilesCv.length > 0 ) {
      perfilSeleccionado = this.perfilesCv.filter( value => value.codigo === perfilCodigo );
      perfilSeleccionado = perfilSeleccionado[0].nombre;
      return perfilSeleccionado;
    };
  };

  innerObservacion ( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    };
  };

};