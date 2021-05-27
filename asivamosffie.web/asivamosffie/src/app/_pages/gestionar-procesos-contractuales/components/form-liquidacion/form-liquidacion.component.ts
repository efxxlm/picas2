import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ProcesosContractualesService } from 'src/app/core/_services/procesosContractuales/procesos-contractuales.service';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { DataSolicitud } from 'src/app/_interfaces/procesosContractuales.interface';

@Component({
  selector: 'app-form-liquidacion',
  templateUrl: './form-liquidacion.component.html',
  styleUrls: ['./form-liquidacion.component.scss']
})
export class FormLiquidacionComponent implements OnInit {
  contratacion: DataSolicitud;
  form         : FormGroup;
  observaciones: string;
  sesionComiteId: number = 0;
  estadoCodigo: string;
  data: any;
  listaTipoDocumento: Dominio[] = [];
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
      nombre            : 'LL000208 - I.E Andrés Bello',
      tipoIntervencion  : 'Remodelación',
      departamento      : 'Valle del Cauca',
      municipio         : 'Jamundi',
      valorTotalProyecto: '80.000.000',
      aportantes        : [
        {
          tipoAportante         : 'ET',
          nombreAportante       : 'Gobernación Del Valle del Cauca',
          valorAportanteProyecto: '30.000.000',
          fuente                : 'Recursos propios',
          valorSolicitado       : '30.000.000'
        },
        {
          tipoAportante         : 'FFIE',
          nombreAportante       : 'FFIE',
          valorAportanteProyecto: '50.000.000',
          fuente                : 'Contingencias',
          valorSolicitado       : '50.000.000'
        }
      ]
    }
  ];

  displayedColumns: string[] = [ 'numeroTraslado', 'ordenGiroAsociada', 'fechaRegistroTraslado', 'valorTraslado', 'aportante', 'aportanteValorFacturado' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Número de traslado', name: 'numeroTraslado' },
    { titulo: 'Orden de giro asociada', name: 'ordenGiroAsociada' },
    { titulo: 'Fecha de registro del traslado', name: 'fechaRegistroTraslado' },
    { titulo: 'Valor del traslado', name: 'valorTraslado' },
    { titulo: 'Aportante que redujo valor facturado', name: 'aportante' },
    { titulo: 'Aportante que aumentó Número de traslado el valor facturado', name: 'aportanteValorFacturado' }
  ];

  displayedColumns1: string[] = [ 'item', 'calificacionInterventoria', 'tipoAnexo', 'ubicacion', 'validacion' ];
  ELEMENT_DATA1    : any[]    = [
    { titulo: 'Ítem', name: 'item' },
    { titulo: 'Calificación Ítem interventoría', name: 'calificacionInterventoria' },
    { titulo: 'Tipo de anexo', name: 'tipoAnexo' },
    { titulo: 'Ubicación (URL/Radicado)', name: 'ubicacion' },
    { titulo: 'Validación', name: 'validacion' }
  ];

  constructor ( private fb: FormBuilder,
                private routes: Router,
                private activatedRoute: ActivatedRoute,
                private procesosContractualesSvc: ProcesosContractualesService,
                private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService,
                private commonSvc: CommonService
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
    this.listaTipoDocumento = await this.commonSvc.listaTipodocumento().toPromise();

    this.registerContractualLiquidationRequestService.getContratacionByContratacionId(contratacionId).subscribe(response => {
      if(response != null){
        this.data = response[0];
        if(this.data.contratacionProyecto?.contratacionId != null){
          this.procesosContractualesSvc.getContratacion(this.data.contratacionProyecto?.contratacionId)
          .subscribe(respuesta => {
            this.contratacion = respuesta;
            let rutaDocumento;
            if ( this.data.contratacionProyecto?.urlSoporteGestionar !== undefined ) {
              rutaDocumento = this.data.contratacionProyecto?.urlSoporteGestionar.split( /[^\w\s]/gi );
              rutaDocumento = `${ rutaDocumento[ rutaDocumento.length -2 ] }.${ rutaDocumento[ rutaDocumento.length -1 ] }`;
            } else {
              rutaDocumento = null;
            };
            this.form.reset({
              fechaEnvioTramite: this.data.contratacionProyecto?.fechaTramiteGestionar,
              observaciones:this.data.contratacionProyecto?.observacionGestionar,
              minutaName: rutaDocumento,
              rutaDocumento: this.data.contratacionProyecto?.urlSoporteGestionar !== null ? this.data.contratacionProyecto?.urlSoporteGestionar : null
            });
          });
        }
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

  getTipoDocumento( codigo: string ) {
    if ( this.listaTipoDocumento.length > 0 ) {
        const tipo = this.listaTipoDocumento.find( tipo => tipo.codigo === codigo );

        if ( tipo !== undefined ) {
            return tipo.nombre;
        }
    }
  }

}
