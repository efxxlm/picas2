import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
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
  sesionComiteId: number = 0;
  estadoCodigo: string;
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
  data: any[] = [
    {
      numeroSolicitud         : 'PI_0039',
      tipoModificacion        : 'Adición, Prorroga',
      valorDespuesModificacion: '90.000.000',
      plazoDespuesModificacion: '20 meses / 5 días',
      detalle                 : 'Se realiza adición de recursos para obras complementarias',
      aportantes              : [
        {
          tipoAportante         : 'FFIE',
          nombreAportante       : 'FFIE',
          valorAportanteProyecto: '150.000.000',
          fuente                : 'Contingencias',
          valorSolicitado       : '150.000.000'
        },
        {
          tipoAportante         : 'ET',
          nombreAportante       : 'Gobernación Del Valle del Cauca',
          valorAportanteProyecto: '150.000.000',
          fuente                : 'Recursos propios',
          valorSolicitado       : '150.000.000'
        }
      ]
    }
  ]

  dataForm: any[] = [
    {
      nombre            : 'LL000012 - I.E Manuela Beltran - Sede Principal',
      tipoIntervencion  : 'Remodelación',
      departamento      : 'Valle del Cauca',
      municipio         : 'Buga',
      valorTotalProyecto: '300.000.000',
      aportantes        : [
        {
          tipoAportante         : 'FFIE',
          nombreAportante       : 'FFIE',
          valorAportanteProyecto: '150.000.000',
          fuente                : 'Contingencias',
          valorSolicitado       : '150.000.000'
        },
        {
          tipoAportante         : 'ET',
          nombreAportante       : 'Gobernación Del Valle del Cauca',
          valorAportanteProyecto: '150.000.000',
          fuente                : 'Recursos propios',
          valorSolicitado       : '150.000.000'
        }
      ]
    }
  ];

  constructor ( private fb: FormBuilder,
                private routes: Router,
                private procesosContractualesSvc: ProcesosContractualesService ) {
    this.crearFormulario();
    this.getMotivo();
  }

  ngOnInit(): void {
  }

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
      minutaFile       : [ null ]
    });
  };

  getDdp ( sesionComiteSolicitudId ) {
    this.procesosContractualesSvc.getDdp( sesionComiteSolicitudId )
      .subscribe();
  };

};