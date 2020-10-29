import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { DataSolicitud } from '../../../../_interfaces/procesosContractuales.interface';
import { CommonService } from '../../../../core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-contratacion',
  templateUrl: './form-contratacion.component.html',
  styleUrls: ['./form-contratacion.component.scss']
})
export class FormContratacionComponent implements OnInit {
  
  form         : FormGroup;
  sesionComiteId: number = 0;
  estadoCodigo: string;
  estadoAprobadoPorFiduciaria: string = '13';
  enviadaFiduciaria: string = '4';
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
                private commonSvc: CommonService,
                private dialog: MatDialog,
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
    return observacionHtml;
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

  getContratacion ( id: number ) {

    this.procesosContractualesSvc.getContratacion( id )
      .subscribe( contratacion => {

        console.log( contratacion );
        this.dataContratacion = contratacion;
        let rutaDocumento;
        if ( contratacion.rutaMinuta !== undefined ) {
          rutaDocumento = contratacion.rutaMinuta.split( /[^\w\s]/gi );
          rutaDocumento = `${ rutaDocumento[ rutaDocumento.length -2 ] }.${ rutaDocumento[ rutaDocumento.length -1 ] }`;
        } else {
          rutaDocumento = null;
        };
        this.form.reset({
          fechaEnvioTramite: contratacion.fechaEnvioDocumentacion,
          observaciones: contratacion.observaciones ? ( contratacion.observaciones.length > 0 ? contratacion.observaciones : null ) : null,
          minutaName: rutaDocumento,
          rutaDocumento: contratacion.rutaMinuta !== null ? contratacion.rutaMinuta : null
        });

        for ( let contratacionProyecto of contratacion.contratacionProyecto ) {
          this.valorTotalDdp += contratacionProyecto.proyecto.valorObra;
          this.valorTotalDdp += contratacionProyecto.proyecto.valorInterventoria;
        };

      } );

  };

  getDdp ( sesionComiteSolicitudId: number, numeroDdp: string ) {
    this.procesosContractualesSvc.getDdp( sesionComiteSolicitudId )
      .subscribe( resp => {
        let documento = '';
        if ( numeroDdp !== undefined ) {
          documento = `${ numeroDdp }.pdf`;
        } else {
          documento = `DDP.pdf`;
        };
        const text = documento,
        blob = new Blob([resp], { type: 'application/pdf' }),
        anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
      } );
  };

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  getDocumento ( nombreDocumento: string ) {
    this.commonSvc.getDocumento( nombreDocumento )
      .subscribe(
        response => {

          const documento = `Minuta contractual`;
          const text = documento,
          blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' }),
          anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = ['application/vnd.openxmlformats-officedocument.wordprocessingml.document', anchor.download, anchor.href].join(':');
          anchor.click();

        },
        err => this.openDialog( '', err.message )
      );
  };

  guardar () {
    console.log( this.form );
  };

};