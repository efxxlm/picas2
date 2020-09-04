import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-contratacion',
  templateUrl: './form-contratacion.component.html',
  styleUrls: ['./form-contratacion.component.scss']
})
export class FormContratacionComponent implements OnInit {
  
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

  dataForm: any[] = [
    {
      nombre: 'LL000007 - I.E De Desarrollo Rural Miguel Valencia - Única sede',
      tipoIntervencion: 'Remodelación',
      departamento: 'Antioquia',
      municipio: 'Bello',
      valorTotalProyecto: '230.000.000',
      aportantes: [
        {
          tipoAportante: 'ET',
          nombreAportante: 'Gobernación de Antioquia',
          valorAportanteProyecto: '150.000.000',
          fuente: 'Recursos propios',
          valorSolicitado: '150.000.000'
        },
        {
          tipoAportante: 'FFIE',
          nombreAportante: 'FFIE',
          valorAportanteProyecto: '80.000.000',
          fuente: 'Contingencias',
          valorSolicitado: '80.000.000'
        },
      ]
    },
    {
      nombre: 'LL000117 - I.E Miguel Suarez - Única sede',
      tipoIntervencion: 'Remodelación',
      departamento: 'Boyacá',
      municipio: 'Paipa',
      valorTotalProyecto: '370.000.000',
      aportantes: [
        {
          tipoAportante: 'ET',
          nombreAportante: 'Gobernación de Boyacá',
          valorAportanteProyecto: '200.000.000',
          fuente: 'Recursos propios',
          valorSolicitado: '200.000.000'
        },
        {
          tipoAportante: 'FFIE',
          nombreAportante: 'FFIE',
          valorAportanteProyecto: '170.000.000',
          fuente: 'Contingencias',
          valorSolicitado: '170.000.000'
        },
      ]
    }
  ]

  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  };

  ngOnInit(): void {
  }

  crearFormulario () {
    this.form = this.fb.group({
      fechaEnvioTramite: [ null, Validators.required ],
      observaciones: [ null, Validators.required ],
      minuta: [ null ],
      minutaFile: [ null ]
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
