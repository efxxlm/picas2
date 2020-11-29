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

              for ( const contratacionProyecto of this.contrato.contratacion.contratacionProyecto ) {
                let semaforoSinDiligenciar = 0;
                let semaforoEnProceso = 0;
                let semaforoCompleto = 0;
                for ( const perfil of contratacionProyecto.proyecto.contratoConstruccion[0].construccionPerfil ) {
                  const observacionTipo3 = [];
                  for ( const observacion of perfil.construccionPerfilObservacion ) {
                    if ( observacion.tipoObservacionCodigo === this.observacionSupervisor ) {
                      observacionTipo3.push( observacion );
                    }
                  }
                  if ( observacionTipo3.length > 0 ) {
                    if (
                      observacionTipo3[ observacionTipo3.length - 1 ].esSupervision !== undefined
                      && observacionTipo3[ observacionTipo3.length - 1 ].esSupervision === false
                    ) {
                      perfil.estadoSemaforo = 'completo';
                      semaforoCompleto++;
                    }
                    if (  observacionTipo3[ observacionTipo3.length - 1 ].esSupervision !== undefined
                          && observacionTipo3[ observacionTipo3.length - 1 ].esSupervision === true
                          && observacionTipo3[ observacionTipo3.length - 1 ].observacion === undefined )
                    {
                      perfil.estadoSemaforo = 'en-proceso';
                      semaforoEnProceso++;
                    }
                    if (  observacionTipo3[ observacionTipo3.length - 1 ].esSupervision !== undefined
                          && observacionTipo3[ observacionTipo3.length - 1 ].esSupervision === true
                          && observacionTipo3[ observacionTipo3.length - 1 ].observacion !== undefined )
                    {
                      perfil.estadoSemaforo = 'completo';
                      semaforoCompleto++;
                    }
                  } else {
                    perfil.estadoSemaforo = 'sin-diligenciar';
                    semaforoSinDiligenciar++;
                  }
                }
                if (  semaforoCompleto > 0
                      && semaforoCompleto === contratacionProyecto.proyecto.contratoConstruccion[0].construccionPerfil.length ) {
                  contratacionProyecto.estadoSemaforoContratacion = 'completo';
                }
                if (  semaforoSinDiligenciar > 0
                      && semaforoSinDiligenciar === contratacionProyecto.proyecto.contratoConstruccion[0].construccionPerfil.length ) {
                  contratacionProyecto.estadoSemaforoContratacion = 'sin-diligenciar';
                }
                if (  semaforoEnProceso > 0
                      && semaforoEnProceso < contratacionProyecto.proyecto.contratoConstruccion[0].construccionPerfil.length ) {
                  contratacionProyecto.estadoSemaforoContratacion = 'en-proceso';
                }
                if (  semaforoCompleto > 0
                      && semaforoSinDiligenciar > 0
                      && semaforoSinDiligenciar + semaforoCompleto
                      === contratacionProyecto.proyecto.contratoConstruccion[0].construccionPerfil.length
                    ) {
                  contratacionProyecto.estadoSemaforoContratacion = 'en-proceso';
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
