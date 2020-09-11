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
  dataForm: any[] = [
    {
      nombre            : 'LL000007 - I.E De Desarrollo Rural Miguel Valencia - Única sede',
      tipoIntervencion  : 'Remodelación',
      departamento      : 'Antioquia',
      municipio         : 'Bello',
      valorTotalProyecto: '230.000.000',
      aportantes        : [
        {
          tipoAportante         : 'ET',
          nombreAportante       : 'Gobernación de Antioquia',
          valorAportanteProyecto: '150.000.000',
          fuente                : 'Recursos propios',
          valorSolicitado       : '150.000.000'
        },
        {
          tipoAportante         : 'FFIE',
          nombreAportante       : 'FFIE',
          valorAportanteProyecto: '80.000.000',
          fuente                : 'Contingencias',
          valorSolicitado       : '80.000.000'
        },
      ]
    },
    {
      nombre            : 'LL000117 - I.E Miguel Suarez - Única sede',
      tipoIntervencion  : 'Remodelación',
      departamento      : 'Boyacá',
      municipio         : 'Paipa',
      valorTotalProyecto: '370.000.000',
      aportantes        : [
        {
          tipoAportante         : 'ET',
          nombreAportante       : 'Gobernación de Boyacá',
          valorAportanteProyecto: '200.000.000',
          fuente                : 'Recursos propios',
          valorSolicitado       : '200.000.000'
        },
        {
          tipoAportante         : 'FFIE',
          nombreAportante       : 'FFIE',
          valorAportanteProyecto: '170.000.000',
          fuente                : 'Contingencias',
          valorSolicitado       : '170.000.000'
        },
      ]
    }
  ]

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
        
        this.form.reset({
          fechaEnvioTramite: contratacion.fechaEnvioDocumentacion,
          observaciones: contratacion.observaciones,
          minutaName: contratacion.rutaMinuta
        });

      } );

  };

  getDdp ( sesionComiteSolicitudId ) {
    this.procesosContractualesSvc.getDdp( sesionComiteSolicitudId )
      .subscribe();
  };

  guardar () {
    console.log( this.form );
  };

};