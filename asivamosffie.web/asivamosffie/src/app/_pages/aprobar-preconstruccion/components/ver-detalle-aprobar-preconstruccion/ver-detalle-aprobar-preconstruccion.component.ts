import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-ver-detalle-aprobar-preconstruccion',
  templateUrl: './ver-detalle-aprobar-preconstruccion.component.html',
  styleUrls: ['./ver-detalle-aprobar-preconstruccion.component.scss']
})
export class VerDetalleAprobarPreconstruccionComponent implements OnInit {

  contrato: Contrato;
  perfilesCv: Dominio[] = [];
  tipoObservaciones = {
    observacionAPoyo: '2',
    observacionSupervisor: '3'
  };

  constructor(
    private activatedRoute: ActivatedRoute,
    private commonSvc: CommonService,
    private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService )
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
    this.commonSvc.listaPerfil()
      .subscribe(
        response => this.perfilesCv = response
      );
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId( pContratoId: string ) {
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( contrato => {
        this.contrato = contrato;
        const observacionesTipo2 = [];
        const observacionesTipo3 = [];
        console.log( this.contrato );
        for ( const contratacionProyecto of contrato.contratacion.contratacionProyecto ) {
          // tslint:disable-next-line: no-string-literal
          for ( const perfil of contratacionProyecto.proyecto[ 'contratoPerfil' ] ) {
            for ( const observacion of perfil.contratoPerfilObservacion ) {
              if ( observacion.tipoObservacionCodigo === this.tipoObservaciones.observacionAPoyo ) {
                observacionesTipo2.push( observacion );
              }
              if ( observacion.tipoObservacionCodigo === this.tipoObservaciones.observacionSupervisor ) {
                observacionesTipo3.push( observacion );
              }
            }
            if ( observacionesTipo2.length > 0 ) {
              // tslint:disable-next-line: no-string-literal
              perfil[ 'observacionApoyo' ] = observacionesTipo2[ observacionesTipo2.length - 1 ];
            }
            if ( observacionesTipo3.length > 0 ) {
              // tslint:disable-next-line: no-string-literal
              perfil[ 'observacionSupervisor' ] = observacionesTipo3[ observacionesTipo3.length - 1 ];
            }
          }
        }
      } );
  }

  tipoPerfil( perfilCodigo: string ) {
    let perfilSeleccionado;
    if ( this.perfilesCv.length > 0 ) {
      perfilSeleccionado = this.perfilesCv.filter( value => value.codigo === perfilCodigo );
      perfilSeleccionado = perfilSeleccionado[0].nombre;
      return perfilSeleccionado;
    }
  }

  innerObservacion( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    }
  }

}
