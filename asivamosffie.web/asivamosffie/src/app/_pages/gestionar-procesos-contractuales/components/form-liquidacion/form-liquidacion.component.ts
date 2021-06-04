import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ActualizarPolizasService } from 'src/app/core/_services/actualizarPolizas/actualizar-polizas.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { ProcesosContractualesService } from 'src/app/core/_services/procesosContractuales/procesos-contractuales.service';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { TipoNovedadCodigo } from 'src/app/_interfaces/estados-novedad.interface';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { DataSolicitud } from 'src/app/_interfaces/procesosContractuales.interface';

@Component({
  selector: 'app-form-liquidacion',
  templateUrl: './form-liquidacion.component.html',
  styleUrls: ['./form-liquidacion.component.scss']
})
export class FormLiquidacionComponent implements OnInit {
  contratacion: DataSolicitud;
  dataNovedadList: NovedadContractual[] = [];
  informeFinal: any[] = [];
  ejecucionPresupuestalList: any[] = [];
  form         : FormGroup;
  observaciones: string;
  sesionComiteId: number = 0;
  estadoCodigo: string;
  polizasYSegurosArray: Dominio[] = [];
  listaTipoSolicitud = {
    obra: '1',
    interventoria: '2'
  };
  contratoPoliza: any;
  contratoPolizaActualizacion: any;
  valorTotalDdp: number = 0;
  seguros = [];
  tipoNovedad = TipoNovedadCodigo;
  data: any;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  displayedColumns: string[] = [ 'numeroTraslado', 'ordenGiroAsociada', 'fechaRegistroTraslado', 'valorTraslado', 'aportante', 'aportanteValorFacturado' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Número de traslado', name: 'numeroTraslado' },
    { titulo: 'Orden de giro asociada', name: 'ordenGiroAsociada' },
    { titulo: 'Fecha de registro del traslado', name: 'fechaRegistroTraslado' },
    { titulo: 'Valor del traslado', name: 'valorTraslado' },
    { titulo: 'Aportante que redujo valor facturado', name: 'aportante' },
    { titulo: 'Aportante que aumentó Número de traslado el valor facturado', name: 'aportanteValorFacturado' }
  ];

  constructor ( private fb: FormBuilder,
                private routes: Router,
                private activatedRoute: ActivatedRoute,
                private procesosContractualesSvc: ProcesosContractualesService,
                private disponibilidadServices:DisponibilidadPresupuestalService,
                private registerContractualLiquidationRequestSvc: RegisterContractualLiquidationRequestService,
                private actualizarPolizaSvc: ActualizarPolizasService,
                private commonSvc: CommonService,
                private financialBalanceService: FinancialBalanceService
                ) {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl || undefined ) {
      this.routes.navigate([ '/procesosContractuales' ]);
      return;
    };
    this.getContratacionByContratacionId( this.activatedRoute.snapshot.params.id );
    this.crearFormulario();
    this.sesionComiteId = this.routes.getCurrentNavigation().extras.state.sesionComiteSolicitudId;
    this.estadoCodigo = this.routes.getCurrentNavigation().extras.state.estadoCodigo;
  }

  ngOnInit(): void {
  }

  async getContratacionByContratacionId(contratacionId: number) {
    this.polizasYSegurosArray = await this.commonSvc.listaGarantiasPolizas().toPromise();
    this.procesosContractualesSvc.getContratacion(contratacionId)
    .subscribe(respuesta => {
      this.contratacion = respuesta;
      //informe final y balance
      if(this.contratacion != null){
        this.contratacion.contratacionProyecto.forEach(r=>{
          //informe final
          this.registerContractualLiquidationRequestSvc.getInformeFinalByProyectoId(r.proyectoId, contratacionId, null).subscribe(report => {
            if(report != null){
              report.forEach(element => {
                this.informeFinal.push({
                  informeFinal: element.informeFinal,
                  llaveMen: r.proyecto?.llaveMen
                });
              })
            }
          });
          //balance
          //ejecución financiera
          this.financialBalanceService.getEjecucionFinancieraXProyectoId(r.proyectoId).subscribe(data => {
            let dataTableEjpresupuestal: any = null;
            let dataTableEjfinanciera: any = null;
            data[0].forEach(element => {
              dataTableEjpresupuestal = {
                facturadoAntesImpuestos: element.facturadoAntesImpuestos,
                nombre: element.nombre,
                porcentajeEjecucionPresupuestal: element.porcentajeEjecucionPresupuestal,
                proyectoId: element.proyectoId,
                saldo: element.saldo,
                tipoSolicitudCodigo: element.tipoSolicitudCodigo,
                totalComprometido: element.totalComprometido
              }
            });
            data[1].forEach(element => {
              dataTableEjfinanciera = {
                nombre: element.nombre,
                ordenadoGirarAntesImpuestos: element.ordenadoGirarAntesImpuestos,
                porcentajeEjecucionFinanciera: element.porcentajeEjecucionFinanciera,
                proyectoId: element.proyectoId,
                saldo: element.saldo,
                totalComprometido: element.totalComprometido
              }
            });
            console.log(dataTableEjfinanciera,dataTableEjpresupuestal);
            const element = {
              dataTableEjpresupuestal: dataTableEjpresupuestal,
              dataTableEjfinanciera: dataTableEjfinanciera
            }
            r.ejecucionPresupuestal = element;
          });
        });
        console.log(this.contratacion.contratacionProyecto);
      }
      let rutaDocumento;
      if ( respuesta?.urlSoporteGestionar !== undefined ) {
        rutaDocumento = respuesta?.urlSoporteGestionar.split( /[^\w\s]/gi );
        rutaDocumento = `${ rutaDocumento[ rutaDocumento.length -2 ] }.${ rutaDocumento[ rutaDocumento.length -1 ] }`;
      } else {
        rutaDocumento = null;
      };
      this.form.reset({
        fechaEnvioTramite: respuesta?.fechaTramiteGestionar,
        observaciones:respuesta.observacionGestionar,
        minutaName: rutaDocumento,
        rutaDocumento: respuesta?.urlSoporteGestionar !== null ? respuesta?.urlSoporteGestionar : null
      });

      if ( this.contratacion.tipoSolicitudCodigo === this.listaTipoSolicitud.obra ) {
        for ( let contratacionProyecto of this.contratacion.contratacionProyecto ) {
          this.valorTotalDdp += contratacionProyecto.proyecto.valorObra;
        };
      }
      if ( this.contratacion.tipoSolicitudCodigo === this.listaTipoSolicitud.interventoria ) {
        for ( let contratacionProyecto of this.contratacion.contratacionProyecto ) {
          this.valorTotalDdp += contratacionProyecto.proyecto.valorInterventoria;
        };
      }

    });
    //novedad
    this.registerContractualLiquidationRequestSvc.getAllNoveltyByContratacion(contratacionId).subscribe(response => {
      if(response != null){
        this.dataNovedadList = response;
        this.dataNovedadList.forEach(dataNovedad => {
          dataNovedad.novedadContractualDescripcion.forEach(element => {
            if(dataNovedad.tipoModificacion == null){
              dataNovedad.tipoModificacion = element.nombreTipoNovedad;
            }else{
              dataNovedad.tipoModificacion = dataNovedad.tipoModificacion + "," + element.nombreTipoNovedad;
            }
            if(element.tipoNovedadCodigo === this.tipoNovedad.adicion)
             dataNovedad.adicionBoolean = true;
        });
        });
      }
    });
    //poliza
    this.registerContractualLiquidationRequestSvc.getPolizaByContratacionId(contratacionId).subscribe(response => {
      if(response != null){
        this.data = response;
        this.actualizarPolizaSvc.getContratoPoliza( this.data.contratoPolizaId )
        .subscribe(
          response => {
            this.contratoPoliza = response;

            if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
                if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                    this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];
                }
            }

            if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
              if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                  this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[0];

                //Razón y tipo de actualización / vigencias
                  if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro !== undefined ) {
                      if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro.length > 0 ) {
                          const polizaGarantia: any[] = this.contratoPoliza.polizaGarantia.length > 0 ? this.contratoPoliza.polizaGarantia : [];
                          for ( const seguro of this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro ) {
                              const seguroPoliza = polizaGarantia.find( seguroPoliza => seguroPoliza.tipoGarantiaCodigo === seguro.tipoSeguroCodigo );

                              //seguros acordeón vgencias y valor
                              const seguroVigencias = {
                                nombre: this.polizasYSegurosArray.find( poliza => poliza.codigo === seguro.tipoSeguroCodigo ).nombre,
                                codigo: seguro.tipoSeguroCodigo,
                                tieneSeguro: seguro.tieneFechaSeguro,
                                fechaSeguro: seguro.fechaSeguro !== undefined ? new Date( seguro.fechaSeguro ) : null,
                                tieneFechaAmparo: seguro.tieneFechaVigenciaAmparo,
                                fechaAmparo: seguro.fechaVigenciaAmparo !== undefined ? new Date( seguro.fechaVigenciaAmparo ) : null,
                                tieneValorAmparo: seguro.tieneValorAmparo,
                                valorAmparo: seguro.valorAmparo !== undefined ? seguro.valorAmparo : null,
                                seguroPoliza,
                                fechaExpedicionActualizacionPoliza: this.contratoPolizaActualizacion !== undefined ? this.contratoPolizaActualizacion?.fechaExpedicionActualizacionPoliza : null,
                              }
                              this.seguros.push(
                                seguroVigencias
                              );
                          }
                      }
                  }
              }
          }
        }
        )
        //this.dataNovedadList = response;
      }
    });
  }

  crearFormulario () {
    this.form = this.fb.group({
      fechaEnvioTramite: [ null, Validators.required ],
      observaciones    : [ null ],
      minuta           : [ null ],
      minutaName       : [ null ],
      minutaFile       : [ null ],
      rutaDocumento    : [ null ]
    })
  };
  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  guardar () {
    console.log( this.form );
  };


  getDdp(disponibilidadPresupuestalId: number, numeroDdp: string ) {
    this.disponibilidadServices.GenerateDDP(disponibilidadPresupuestalId, false, 0,false).subscribe((listas:any) => {
      console.log(listas);
      let documento = '';
        if ( numeroDdp !== undefined ) {
          documento = `${ numeroDdp }.pdf`;
        } else {
          documento = `DDP.pdf`;
        };
        const text = documento,
          blob = new Blob([listas], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
    });
  }
}
