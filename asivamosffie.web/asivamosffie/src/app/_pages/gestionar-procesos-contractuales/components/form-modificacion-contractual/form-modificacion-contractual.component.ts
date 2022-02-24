import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, TiposAportante } from 'src/app/core/_services/common/common.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { TipoNovedadCodigo } from 'src/app/_interfaces/estados-novedad.interface';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { DataSolicitud } from 'src/app/_interfaces/procesosContractuales.interface';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';

@Component({
  selector: 'app-form-modificacion-contractual',
  templateUrl: './form-modificacion-contractual.component.html',
  styleUrls: ['./form-modificacion-contractual.component.scss']
})
export class FormModificacionContractualComponent implements OnInit {

  form             : FormGroup;
  archivo          : string;
  observaciones    : string;
  reinicioBoolean  : boolean;
  suspensionBoolean: boolean;
  adicionBoolean : boolean = false;
  sesionComiteId: number = 0;
  estadoCodigo: string;
  dataNovedad: NovedadContractual;
  tipoModificacion : string;
  valorTotalDdp: number = 0;
  contratacion: DataSolicitud;
  tipoAportante = TiposAportante;
  tipoNovedad = TipoNovedadCodigo;
  novedadContractualRegistroPresupuestalId: number = 0;

  listaTipoSolicitud = {
    obra: '1',
    interventoria: '2'
  };
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

  constructor ( private fb: FormBuilder,
                private routes: Router,
                private procesosContractualesSvc: ProcesosContractualesService,
                private disponibilidadServices:DisponibilidadPresupuestalService,
                private activatedRoute: ActivatedRoute,
                private commonSvc: CommonService
                ) {
    this.getNovedadById( this.activatedRoute.snapshot.params.id );
    this.crearFormulario();
    //this.getMotivo();
  }

  ngOnInit(): void {
    this.procesosContractualesSvc.getNovedadById( this.activatedRoute.snapshot.params.id )
      .subscribe( respuesta => {
        //console
      })
  }

  getNovedadById ( id: number ) {

    this.procesosContractualesSvc.getNovedadById( id )
      .subscribe( novedadContractual => {
        this.dataNovedad = novedadContractual;
        this.dataNovedad.novedadContractualDescripcion.forEach(element => {
            if(this.tipoModificacion == null){
              this.tipoModificacion = element.nombreTipoNovedad;
            }else{
              this.tipoModificacion = this.tipoModificacion + "," + element.nombreTipoNovedad;
            }
            if(element.tipoNovedadCodigo === this.tipoNovedad.adicion)
              this.adicionBoolean = true;
            if(element.tipoNovedadCodigo === this.tipoNovedad.prorroga){
              let plazoRes = this.commonSvc.plazoDespuesModificacion(element?.plazoAdicionalDias, element?.plazoAdicionalMeses, this.dataNovedad?.contrato?.contratacion?.plazoContratacion?.plazoMeses, this.dataNovedad?.contrato?.contratacion?.plazoContratacion?.plazoDias);
              if(plazoRes != null){
                element.plazoModificacionDias =  plazoRes.plazoModificacionDias;
                element.plazoModificacionMeses = plazoRes.plazoModificacionMeses;
              }
            }
        });
        let rutaDocumento;
        if ( novedadContractual.urlSoporteGestionar !== undefined ) {
          rutaDocumento = novedadContractual.urlSoporteGestionar.split( /[^\w\s]/gi );
          rutaDocumento = `${ rutaDocumento[ rutaDocumento.length -2 ] }.${ rutaDocumento[ rutaDocumento.length -1 ] }`;
        } else {
          rutaDocumento = null;
        };

        this.form.reset({
          fechaEnvioTramite: novedadContractual.fechaTramiteGestionar,
          observaciones:novedadContractual.observacionGestionar,
          minutaName: rutaDocumento,
          rutaDocumento: novedadContractual.urlSoporteGestionar !== null ? novedadContractual.urlSoporteGestionar : null
        });

        if ( novedadContractual.contrato.contratacion.tipoSolicitudCodigo === this.listaTipoSolicitud.obra ) {
          for ( let contratacionProyecto of novedadContractual.contrato.contratacion.contratacionProyecto ) {
            this.valorTotalDdp += contratacionProyecto.proyecto.valorObra;
          };
        }
        if ( novedadContractual.contrato.contratacion.tipoSolicitudCodigo === this.listaTipoSolicitud.interventoria ) {
          for ( let contratacionProyecto of novedadContractual.contrato.contratacion.contratacionProyecto ) {
            this.valorTotalDdp += contratacionProyecto.proyecto.valorInterventoria;
          };
        }
        if(this.dataNovedad.contrato?.contratacionId != null){
          this.procesosContractualesSvc.getContratacion(this.dataNovedad.contrato?.contratacionId)
          .subscribe(respuesta => {
            this.contratacion = respuesta;
          });
        }
        this.novedadContractualRegistroPresupuestalId = this.dataNovedad?.novedadContractualRegistroPresupuestal[0]?.novedadContractualRegistroPresupuestalId;
        this.estadoCodigo = this.dataNovedad.estadoCodigo;
      });

  };

  getMotivo () {

    if ( this.routes.getCurrentNavigation().extras.replaceUrl || undefined ) {
      this.reinicioBoolean   = false;
      this.suspensionBoolean = false;
      return;
    };
    this.sesionComiteId = this.routes.getCurrentNavigation().extras.state.sesionComiteSolicitudId;
    this.estadoCodigo = this.routes.getCurrentNavigation().extras.state.estadoCodigo;
    this.suspensionBoolean = this.routes.getCurrentNavigation().extras.state.suspension;
    this.reinicioBoolean   = this.routes.getCurrentNavigation().extras.state.reinicio;

    if ( this.reinicioBoolean ) {
      //reiniciar data formulario
      this.form.reset({
        fechaEnvioTramite: [ null ],
        observaciones    : [ null ],
        minutaFile       : [ null ]
      });
    }

  };

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

  getDdp(disponibilidadPresupuestalId: number, numeroDdp: string ) {
    this.disponibilidadServices.GenerateDDP(disponibilidadPresupuestalId, true, this.novedadContractualRegistroPresupuestalId,false,false).subscribe((listas:any) => {
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

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion?.replace( '"', '' );
    return observacionHtml;
  }


};
