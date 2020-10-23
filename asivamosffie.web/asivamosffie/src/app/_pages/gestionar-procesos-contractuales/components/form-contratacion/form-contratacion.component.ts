import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { DataSolicitud } from '../../../../_interfaces/procesosContractuales.interface';

@Component({
  selector: 'app-form-contratacion',
  templateUrl: './form-contratacion.component.html',
  styleUrls: ['./form-contratacion.component.scss']
})
export class FormContratacionComponent implements OnInit {
  
  form         : FormGroup;
  sesionComiteId: number = 0;
  estadoCodigo: string;
  valorTotalDdp: number = 0;
  dataContratacion: DataSolicitud = {
    contratacionId: 0,
    consideracionDescripcion: '',
    tipoSolicitudCodigo: '',
    numeroSolicitud: '',
    estadoSolicitudCodigo: '',
    contratistaId: 0,
    usuarioCreacion: '',
    fechaCreacion: '',
    eliminado: false,
    fechaEnvioDocumentacion: '',
    observaciones: '',
    rutaMinuta: '',
    fechaTramite: '',
    tipoContratacionCodigo: '',
    registroCompleto: true,
    contratista: null,
    contratacionProyecto: null,
    contrato: null,
    disponibilidadPresupuestal: null,
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
                private activatedRoute: ActivatedRoute,
                private routes: Router,
                private procesosContractualesSvc: ProcesosContractualesService ) {
    this.getContratacion( this.activatedRoute.snapshot.params.id );
    this.crearFormulario();
    if ( this.routes.getCurrentNavigation().extras.replaceUrl || undefined ) {
      this.routes.navigate([ '/procesosContractuales' ]);
      return;
    };
    this.sesionComiteId = this.routes.getCurrentNavigation().extras.state.sesionComiteSolicitudId;
    this.estadoCodigo = this.routes.getCurrentNavigation().extras.state.estadoCodigo;
  };

  ngOnInit(): void {
  };

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml
  }

  crearFormulario () {
    this.form = this.fb.group({
      fechaEnvioTramite: [ null, Validators.required ],
      observaciones    : [ null ],
      minuta           : [ null ],
      minutaName       : [ null ],
      minutaFile       : [ null ]
    })
  };

  getContratacion ( id: number ) {

    this.procesosContractualesSvc.getContratacion( id )
      .subscribe( contratacion => {

        console.log( contratacion );
        this.dataContratacion = contratacion;
        let rutaDocumento;
        if ( contratacion.rutaMinuta !== undefined ) {
          rutaDocumento = contratacion.rutaMinuta.split( '/' );
        } else {
          rutaDocumento = null;
        };
        this.form.reset({
          fechaEnvioTramite: contratacion.fechaEnvioDocumentacion,
          observaciones: contratacion.observaciones ? ( contratacion.observaciones.length > 0 ? contratacion.observaciones : null ) : null,
          minutaName: rutaDocumento[ rutaDocumento.length-1 ]
        });

        for ( let contratacionProyecto of contratacion.contratacionProyecto ) {
          this.valorTotalDdp += contratacionProyecto.proyecto.valorObra;
          this.valorTotalDdp += contratacionProyecto.proyecto.valorInterventoria;
        };

      } );

  };

  getDdp ( sesionComiteSolicitudId, numeroDdp ) {
    this.procesosContractualesSvc.getDdp( sesionComiteSolicitudId )
      .subscribe( resp => {
        const documento = `DDP ${ numeroDdp }.pdf`;
        const text = documento,
        blob = new Blob([resp], { type: 'application/pdf' }),
        anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
      } );
  };

  guardar () {
    console.log( this.form );
  };

};