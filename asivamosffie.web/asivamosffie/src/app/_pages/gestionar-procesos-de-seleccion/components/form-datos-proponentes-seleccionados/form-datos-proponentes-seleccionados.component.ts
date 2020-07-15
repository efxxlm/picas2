import { Component } from '@angular/core';
import { FormBuilder, Validators, FormControl, FormArray, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-form-datos-proponentes-seleccionados',
  templateUrl: './form-datos-proponentes-seleccionados.component.html',
  styleUrls: ['./form-datos-proponentes-seleccionados.component.scss']
})
export class FormDatosProponentesSeleccionadosComponent {

  tipoProponente: FormControl;

  proponentes = [
    {name: 'Persona natural', value: 1},
    {name: 'Persona jurídica - individual', value: 2},
    {name: 'Unión Temporal o Consorcio', value: 3}
  ];


  personaNaturalForm = this.fb.group({
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
    ],
    numeroIdentificacion: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    telefono: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    correoElectronico: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ]
  });

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

  personaJuridicaIndividualForm = this.fb.group({
    cuantasEntidades: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2)])
    ],
    entidades: this.fb.array([]),

    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
    ],
    numeroIdentificacion: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    telefono: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    correoElectronico: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ]
  });

  get entidades() {
    return this.personaJuridicaIndividualForm.get('entidades') as FormArray;
  }


  constructor(private fb: FormBuilder) {
    this.declararSelect();
  }

  private declararSelect() {
    this.tipoProponente = new FormControl('', [Validators.required]);
  }

  CambioNumeroCotizantes() {
    const formIntegrantes = this.personaJuridicaIndividualForm.value;
    if (formIntegrantes.cuantasCotizaciones > this.entidades.length && formIntegrantes.cuantasCotizaciones < 100) {
      while (this.entidades.length < formIntegrantes.cuantasCotizaciones) {
        this.entidades.push( this.createIntegrante() );
      }
    } else if (formIntegrantes.cuantasCotizaciones <= this.entidades.length && formIntegrantes.cuantasCotizaciones >= 0) {
      while (this.entidades.length > formIntegrantes.cuantasCotizaciones) {
        this.borrarArray(this.entidades, this.entidades.length - 1);
      }
    }
  }

  createIntegrante(): FormGroup {
    return this.fb.group({
      nombre: [null, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(100)])
      ],
      porcentaje: [null, Validators.compose([
        Validators.required, Validators.min(1), Validators.max(100)])
      ]
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmitPersonaNatural() {
    if (this.personaNaturalForm.valid) {
      console.log(this.personaNaturalForm.value);
    }
  }

  onSubmitPersonaJuridicaIndividual() {
    if (this.personaNaturalForm.valid) {
      console.log(this.personaNaturalForm.value);
    }
  }
}
