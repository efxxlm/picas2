import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CofinanciacionService, CofinanciacionAportante } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';

@Component({
  selector: 'app-registrar-acuerdo',
  templateUrl: './registrar-acuerdo.component.html',
  styleUrls: ['./registrar-acuerdo.component.scss']
})
export class RegistrarAcuerdoComponent {

  mostrarDocumentosDeApropiacion = true;
  maxDate: Date;

  constructor(private fb: FormBuilder,
              private cofinanciacionService: CofinanciacionService) {
    this.maxDate = new Date();

    this.datosAportantes.get('numAportes').valueChanges
    .subscribe( () => {
      this.CambioNumeroAportantes();
    });
  }

  get aportantes() {
    return this.datosAportantes.get('aportantes') as FormArray;
  }

  get DocumentoAportantes() {
    return this.documentoApropiacion.get('aportantes') as FormArray;
  }

  datosAportantes = this.fb.group({
    vigenciaEstado: ['', Validators.required],
    numAportes: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    aportantes: this.fb.array([])
  });

  vigenciaEstados = [2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024];
  selectTiposAportante = [
    { name: 'FFIE', value: 1 }, { name: 'ET', value: 2 }, { name: 'Tercero/otro', value: 3 }
  ];
  nombresAportante = [
    { name: 'fundacion 1', value: 1 }, { name: 'fundacion 2', value: 2 }, { name: 'fundacion 3', value: 3 }
  ];


  // tabla de los documentos de aportantes
  documentoApropiacion = this.fb.group({
    aportantes: this.fb.array([])
  });

  valorTotalAcuerdo = 85000000;

  vigenciasAportante = [2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024];
  tiposDocumento = [
    { name: 'Resolución 1', value: 1 }, { name: 'Resolución 2', value: 2 }, { name: 'Resolución 3', value: 3 }
  ];

  CambioNumeroAportantes(){
    const FormNumAportantes = this.datosAportantes.value;
    if (FormNumAportantes.numAportes > this.aportantes.length && FormNumAportantes.numAportes < 100) {
      while (this.aportantes.length < FormNumAportantes.numAportes) {
        this.aportantes.push( this.createAportante() );
        this.DocumentoAportantes.push( this.createDocumentoAportante() );
      }
    } else if (FormNumAportantes.numAportes <= this.aportantes.length && FormNumAportantes.numAportes >= 0) {
      while (this.aportantes.length > FormNumAportantes.numAportes) {
        this.borrarAportante(this.aportantes, this.aportantes.length - 1);
        this.borrarAportante(this.DocumentoAportantes, this.aportantes.length - 1);
      }
    }
  }

  createAportante(): FormGroup {
    return this.fb.group({
      tipo: ['', Validators.required],
      nombre: ['', Validators.required],
      cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    });
  }

  createDocumentoAportante(): FormGroup {
    return this.fb.group({
      vigenciaAportante: [null, Validators.required],
      valorIndicadoEnElDocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ],
      tipoDocumento: [null, Validators.required],
      numeroDocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(10), Validators.maxLength(10)])
      ],
      fechaDocumento: [null, Validators.required]
    });
  }


  borrarAportante(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmitDatosAportantes() {
    if (this.datosAportantes.valid) {
      console.log(this.datosAportantes.value);
      this.mostrarDocumentosDeApropiacion = true;
    }
  }

  onSubmitDocumentosAportantes() {
    console.log(this.documentoApropiacion.value);
  }
}
