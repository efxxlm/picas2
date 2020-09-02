import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-modificacion-contractual',
  templateUrl: './form-modificacion-contractual.component.html',
  styleUrls: ['./form-modificacion-contractual.component.scss']
})
export class FormModificacionContractualComponent implements OnInit {

  form: FormGroup;
  observaciones: string;
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
      numeroSolicitud: 'PI_0039',
      tipoModificacion: 'Adición, Prorroga',
      valorDespuesModificacion: '90.000.000',
      plazoDespuesModificacion: '20 meses / 5 días',
      detalle: 'Se realiza adición de recursos para obras complementarias',
      aportantes: [
        {
          tipoAportante: 'FFIE',
          nombreAportante: 'FFIE',
          valorAportanteProyecto: '150.000.000',
          fuente: 'Contingencias',
          valorSolicitado: '150.000.000'
        },
        {
          tipoAportante: 'ET',
          nombreAportante: 'Gobernación Del Valle del Cauca',
          valorAportanteProyecto: '150.000.000',
          fuente: 'Recursos propios',
          valorSolicitado: '150.000.000'
        }
      ]
    }
  ]

  dataForm: any[] = [
    {
      nombre: 'LL000012 - I.E Manuela Beltran - Sede Principal',
      tipoIntervencion: 'Remodelación',
      departamento: 'Valle del Cauca',
      municipio: 'Buga',
      valorTotalProyecto: '300.000.000',
      aportantes: [
        {
          tipoAportante: 'FFIE',
          nombreAportante: 'FFIE',
          valorAportanteProyecto: '150.000.000',
          fuente: 'Contingencias',
          valorSolicitado: '150.000.000'
        },
        {
          tipoAportante: 'ET',
          nombreAportante: 'Gobernación Del Valle del Cauca',
          valorAportanteProyecto: '150.000.000',
          fuente: 'Recursos propios',
          valorSolicitado: '150.000.000'
        }
      ]
    }
  ];

  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
  }

  crearFormulario () {
    this.form = this.fb.group({
      fechaEnvioTramite: [ null, Validators.required ],
      observaciones: [ null, Validators.required ],
      minuta: [ null ]
    })
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  guardar () {
    console.log( this.form );
  }

}
