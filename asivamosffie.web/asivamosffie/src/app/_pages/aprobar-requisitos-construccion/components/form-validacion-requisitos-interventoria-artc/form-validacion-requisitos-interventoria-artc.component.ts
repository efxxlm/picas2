import { Contrato } from './../../../../_interfaces/faseUnoPreconstruccion.interface';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-validacion-requisitos-interventoria-artc',
  templateUrl: './form-validacion-requisitos-interventoria-artc.component.html',
  styleUrls: ['./form-validacion-requisitos-interventoria-artc.component.scss']
})
export class FormValidacionRequisitosInterventoriaArtcComponent implements OnInit {

  contrato: Contrato;
  perfilesCv: Dominio[] = [];
  observacionSupervisor = '3';

  constructor(
    private faseUnoConstruccionService: FaseUnoConstruccionService,
    private commonSvc: CommonService,
    private activatedRoute: ActivatedRoute
  )
  {
    this.getContrato();
  }

  ngOnInit(): void {
  }

  getContrato() {
    this.commonSvc.listaPerfil()
      .subscribe(
        perfiles => {
          this.perfilesCv = perfiles;
          this.faseUnoConstruccionService.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe( response => {
              this.contrato = response;
              console.log( this.contrato );

              for ( const contratacion of this.contrato.contratacion.contratacionProyecto ) {

                let perfilSinDiligenciar = 0;
                let perfilEnProceso = 0;
                let perfilCompleto = 0;
                for ( const perfil of contratacion.proyecto.contratoConstruccion[0].construccionPerfil ) {
                  perfil.semaforoPerfil = 'sin-diligenciar';
                  if  ( perfil.tieneObservacionesSupervisor !== undefined
                        && (  perfil.tieneObservacionesSupervisor === true
                              || perfil.tieneObservacionesSupervisor === false )
                  )
                  {
                    perfil.semaforoPerfil = 'completo';
                    if (  perfil.observacionSupervisor !== undefined
                          && perfil.observacionSupervisor.observacion === undefined
                          && perfil.tieneObservacionesSupervisor === true )
                    {
                      perfil.semaforoPerfil = 'en-proceso';
                    }
                  }
                  if ( perfil.semaforoPerfil === 'sin-diligenciar' ) {
                    perfilSinDiligenciar++;
                  }
                  if ( perfil.semaforoPerfil === 'en-proceso' ) {
                    perfilEnProceso++;
                  }
                  if ( perfil.semaforoPerfil === 'completo' ) {
                    perfilCompleto++;
                  }
                }
                if (  perfilSinDiligenciar > 0
                      && perfilSinDiligenciar === contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length )
                {
                  contratacion[ 'estadoSemaforo' ] = 'sin-diligenciar';
                }
                if (  perfilCompleto > 0
                      && perfilCompleto === contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length )
                {
                  contratacion[ 'estadoSemaforo' ] = 'completo';
                }
                if ( perfilEnProceso > 0 ) {
                  contratacion[ 'estadoSemaforo' ] = 'en-proceso';
                }
                if (  perfilSinDiligenciar > 0
                      && perfilSinDiligenciar < contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length )
                {
                  contratacion[ 'estadoSemaforo' ] = 'en-proceso';
                }
                if (  perfilCompleto > 0
                      && perfilCompleto < contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length )
                {
                  contratacion[ 'estadoSemaforo' ] = 'en-proceso';
                }
              }
            } );
        }
      );
  }

  getTipoPerfil( perfilCodigo: string ) {
    if ( this.perfilesCv.length > 0 ) {
      const tipoPerfil = this.perfilesCv.filter( value => value.codigo === perfilCodigo );
      return tipoPerfil[0].nombre;
    }
  }

  innerObservacion( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    }
  }

  recargarComponente( seRealizoPeticion: boolean ) {
    if ( seRealizoPeticion === true ) {
      this.contrato = null;
      this.getContrato();
    }
  }

}
