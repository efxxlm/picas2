import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';

@Component({
  selector: 'app-form-descripcion-del-proceso-de-seleccion',
  templateUrl: './form-descripcion-del-proceso-de-seleccion.component.html',
  styleUrls: ['./form-descripcion-del-proceso-de-seleccion.component.scss']
})

export class FormDescripcionDelProcesoDeSeleccionComponent {

  addressForm = this.fb.group({
    objeto: [null, Validators.required],
    alcanceParticular: [null, Validators.required],
    justificacion: [null, Validators.required],
    tipoIntervencion: [null, Validators.required],
    tipoAlcance: [null, Validators.required],
    distribucionEnGrupos: ['free', Validators.required],
    cuantosGrupos: [null, Validators.compose([
      Validators.minLength(1), Validators.maxLength(2)])
    ],
    grupos: this.fb.array([
      this.fb.group({
        nombreGrupo: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(100)])
        ],
        tipoPresupuesto: [null, Validators.required],
        plazoMeses: [null, Validators.compose([
          Validators.required, Validators.minLength(1), Validators.maxLength(2)])
        ]
      })
    ]),
    cronogramas: this.fb.array([
      this.fb.group({
        descripcion: [null, Validators.required],
        fechaMaxima: [null, Validators.required]
      })
    ]),
  });

  get grupos() {
    return this.addressForm.get('grupos') as FormArray;
  }

  get cronogramas() {
    return this.addressForm.get('cronogramas') as FormArray;
  }

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  states = [
    {name: 'Alabama', abbreviation: 'AL'},
    {name: 'Alaska', abbreviation: 'AK'},
    {name: 'American Samoa', abbreviation: 'AS'},
    {name: 'Arizona', abbreviation: 'AZ'},
    {name: 'Arkansas', abbreviation: 'AR'},
    {name: 'California', abbreviation: 'CA'},
    {name: 'Colorado', abbreviation: 'CO'},
    {name: 'Connecticut', abbreviation: 'CT'},
    {name: 'Delaware', abbreviation: 'DE'},
    {name: 'District Of Columbia', abbreviation: 'DC'},
    {name: 'Federated States Of Micronesia', abbreviation: 'FM'},
    {name: 'Florida', abbreviation: 'FL'},
    {name: 'Georgia', abbreviation: 'GA'},
    {name: 'Guam', abbreviation: 'GU'},
    {name: 'Hawaii', abbreviation: 'HI'},
    {name: 'Idaho', abbreviation: 'ID'},
    {name: 'Illinois', abbreviation: 'IL'},
    {name: 'Indiana', abbreviation: 'IN'},
    {name: 'Iowa', abbreviation: 'IA'},
    {name: 'Kansas', abbreviation: 'KS'},
    {name: 'Kentucky', abbreviation: 'KY'},
    {name: 'Louisiana', abbreviation: 'LA'},
    {name: 'Maine', abbreviation: 'ME'},
    {name: 'Marshall Islands', abbreviation: 'MH'},
    {name: 'Maryland', abbreviation: 'MD'},
    {name: 'Massachusetts', abbreviation: 'MA'},
    {name: 'Michigan', abbreviation: 'MI'},
    {name: 'Minnesota', abbreviation: 'MN'},
    {name: 'Mississippi', abbreviation: 'MS'},
    {name: 'Missouri', abbreviation: 'MO'},
    {name: 'Montana', abbreviation: 'MT'},
    {name: 'Nebraska', abbreviation: 'NE'},
    {name: 'Nevada', abbreviation: 'NV'},
    {name: 'New Hampshire', abbreviation: 'NH'},
    {name: 'New Jersey', abbreviation: 'NJ'},
    {name: 'New Mexico', abbreviation: 'NM'},
    {name: 'New York', abbreviation: 'NY'},
    {name: 'North Carolina', abbreviation: 'NC'},
    {name: 'North Dakota', abbreviation: 'ND'},
    {name: 'Northern Mariana Islands', abbreviation: 'MP'},
    {name: 'Ohio', abbreviation: 'OH'},
    {name: 'Oklahoma', abbreviation: 'OK'},
    {name: 'Oregon', abbreviation: 'OR'},
    {name: 'Palau', abbreviation: 'PW'},
    {name: 'Pennsylvania', abbreviation: 'PA'},
    {name: 'Puerto Rico', abbreviation: 'PR'},
    {name: 'Rhode Island', abbreviation: 'RI'},
    {name: 'South Carolina', abbreviation: 'SC'},
    {name: 'South Dakota', abbreviation: 'SD'},
    {name: 'Tennessee', abbreviation: 'TN'},
    {name: 'Texas', abbreviation: 'TX'},
    {name: 'Utah', abbreviation: 'UT'},
    {name: 'Vermont', abbreviation: 'VT'},
    {name: 'Virgin Islands', abbreviation: 'VI'},
    {name: 'Virginia', abbreviation: 'VA'},
    {name: 'Washington', abbreviation: 'WA'},
    {name: 'West Virginia', abbreviation: 'WV'},
    {name: 'Wisconsin', abbreviation: 'WI'},
    {name: 'Wyoming', abbreviation: 'WY'}
  ];

  constructor(private fb: FormBuilder) { }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  CambioNumeroAportantes() {
    const FormGrupos = this.addressForm.value;
    if (FormGrupos.cuantosGrupos > this.grupos.length && FormGrupos.cuantosGrupos < 100) {
      while (this.grupos.length < FormGrupos.cuantosGrupos) {
        this.grupos.push( this.createGrupo() );
      }
    } else if (FormGrupos.cuantosGrupos <= this.grupos.length && FormGrupos.cuantosGrupos >= 0) {
      while (this.grupos.length > FormGrupos.cuantosGrupos) {
        this.borrarArray(this.grupos, this.grupos.length - 1);
      }
    }
  }

  createGrupo(): FormGroup {
    return this.fb.group({
      nombreGrupo: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      tipoPresupuesto: [null, Validators.required],
      plazoMeses: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(2)])
      ]
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregarActividad(): FormGroup {
    return this.fb.group({
      descripcion: [null, Validators.required],
        fechaMaxima: [null, Validators.required]
      });
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
