import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
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
  perfilesCv: Dominio[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
    private commonSvc: CommonService )
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId( pContratoId: string ) {
    this.commonSvc.listaPerfil()
      .subscribe(
        perfiles => this.perfilesCv = perfiles
      );
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( contrato => {
        this.contrato = contrato;
        console.log( this.contrato );
      } );
  }

  innerObservacion( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    }
  }

  getNombrePerfil( perfilCodigo: string ) {
    if ( this.perfilesCv.length > 0 ) {
      const perfil = this.perfilesCv.filter( value => value.codigo === perfilCodigo );
      return perfil[0].nombre;
    }
  }

}
