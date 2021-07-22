import { Component, Input, Output, EventEmitter, OnInit, SimpleChanges, OnChanges } from '@angular/core';
import { FormBuilder, Validators, FormControl, FormArray, FormGroup } from '@angular/forms';
import {
  ProcesoSeleccion,
  ProcesoSeleccionProponente,
  ProcesoSeleccionIntegrante,
  ProcesoSeleccionService
} from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { Dominio, Localizacion, CommonService } from 'src/app/core/_services/common/common.service';
import { forkJoin, Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-form-datos-proponentes-nuevo',
  templateUrl: './form-datos-proponentes-nuevo.component.html',
  styleUrls: ['./form-datos-proponentes-nuevo.component.scss']
})
export class FormDatosProponentesNuevoComponent implements OnInit, OnChanges {
  @Input() procesoSeleccion: ProcesoSeleccion;
  @Input() noTanNuevo: boolean = false;
  @Input() amountNuevosProponentes: number;
  @Input() procesoSeleccionProponente: any;
  @Output() guardar: EventEmitter<any> = new EventEmitter();

  listaDepartamentos: Localizacion[] = [];
  listaMunicipios = [];
  listaProponentes: Dominio[] = [];
  tipoProponente: FormControl;
  myControl = new FormControl();
  myJuridica = new FormControl();
  myJuridica2 = new FormControl();
  cuantasEntidades = new FormControl();

  addressForm = this.fb.group({
    proponentes: this.fb.array([])
  });

  get proponentesField() {
    return this.addressForm.get('proponentes') as FormArray;
  }

  getEntidades(i: string | number) {
    return this.proponentesField.controls[i].get('procesoSeleccionIntegrante') as FormArray;
  }

  agregarProponentes() {
    this.proponentesField.push(this.crearProponentes());
  }

  private crearProponentes() {
    return this.fb.group({
      tipoProponenteCodigo: [null, Validators.required],

      nombreProponente: [null, Validators.compose([Validators.minLength(2), Validators.maxLength(1000)])],
      procesoSeleccionProponenteId: [],
      numeroIdentificacion: [
        null,
        Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(12)])
      ],
      nombreDepartamento: [null, Validators.required],

      localizacionIdMunicipio: [null, Validators.required],
      direccionProponente: [null, Validators.compose([Validators.required, Validators.maxLength(500)])],
      telefonoProponente: [
        null,
        Validators.compose([Validators.required, Validators.minLength(7), Validators.maxLength(10)])
      ],
      emailProponente: [
        null,
        Validators.compose([
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(1000),
          Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
        ])
      ],
      procesoSeleccionId: [this.procesoSeleccion.procesoSeleccionId, Validators.required],
      nombreRepresentanteLegal: [null, Validators.required],
      cedulaRepresentanteLegal: [null, Validators.required],
      cuantasEntidades: [null, Validators.required],
      procesoSeleccionIntegrante: this.fb.array([]),
      tipoIdentificacionCodigo: ['1', Validators.required]
    });
  }

  personaNaturalForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    nombre: [null, Validators.compose([Validators.minLength(2), Validators.maxLength(1000)])],
    numeroIdentificacion: [
      null,
      Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([Validators.required, Validators.maxLength(500)])],
    telefono: [null, Validators.compose([Validators.required, Validators.minLength(7), Validators.maxLength(10)])],
    correoElectronico: [
      null,
      Validators.compose([
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(1000),
        // Validators.email,
        Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
      ])
    ]
  });

  personaJuridicaIndividualForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    nombre: [null, Validators.compose([Validators.minLength(2), Validators.maxLength(1000)])],
    numeroIdentificacion: [
      null,
      Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    representanteLegal: [
      null,
      Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(1000)])
    ],
    cedulaRepresentanteLegal: [
      null,
      Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([Validators.required, Validators.maxLength(500)])],
    telefono: [null, Validators.compose([Validators.required, Validators.minLength(7), Validators.maxLength(10)])],
    correoElectronico: [
      null,
      Validators.compose([
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(1000),
        // Validators.email,
        Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
      ])
    ]
  });

  unionTemporalForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    cuantasEntidades: [null, Validators.compose([Validators.required])],
    nombreConsorcio: [null, Validators.compose([Validators.minLength(2), Validators.maxLength(1000)])],
    procesoSeleccionIntegrante: this.fb.array([]),
    nombre: [null, Validators.compose([Validators.minLength(2), Validators.maxLength(100)])],
    numeroIdentificacion: [null, Validators.compose([Validators.required, Validators.maxLength(12)])],
    cedulaRepresentanteLegal: [null, Validators.compose([Validators.required, Validators.maxLength(12)])],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([Validators.required, Validators.maxLength(500)])],
    telefono: [null, Validators.compose([Validators.required, Validators.minLength(7), Validators.maxLength(10)])],
    correoElectronico: [
      null,
      Validators.compose([
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(1000),
        // Validators.email,
        Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
      ])
    ]
  });
  listaProponentesNombres: any[] = [];
  nombresapo: string[] = [];
  filteredNameJuridica2: Observable<string[]>;
  estaEditando = false;

  filteredName: Observable<string[]>;
  filteredNameJuridica: Observable<string[]>;

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    public dialog: MatDialog,
    private procesoSeleccionService: ProcesoSeleccionService
  ) {
    this.declararSelect();
  }
  ngOnInit() {
    return new Promise<void>(resolve => {
      forkJoin([
        this.commonService.listaTipoProponente(),
        this.commonService.listaDepartamentos(),
        this.procesoSeleccionService.getProcesoSeleccionProponentes()
      ]).subscribe(respuesta => {
        this.listaProponentes = respuesta[0];
        this.listaDepartamentos = respuesta[1];
        this.listaProponentesNombres = respuesta[2];
        respuesta[2].forEach(element => {
          if (element.nombreProponente) {
            if (!this.nombresapo.includes(element.nombreProponente)) {
              this.nombresapo.push(element.nombreProponente);
            }
          }
        });
        this.filteredName = this.myControl.valueChanges.pipe(
          startWith(''),
          map(value => this._filter(value))
        );
        this.filteredNameJuridica = this.myJuridica.valueChanges.pipe(
          startWith(''),
          map(value => this._filter(value))
        );
        this.filteredNameJuridica2 = this.myJuridica2.valueChanges.pipe(
          startWith(''),
          map(value => this._filter2(value))
        );

        resolve();
      });
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.procesoSeleccionProponente.previousValue) {
      let differenceAddingProponente = this.differenceDeArray(
        changes.procesoSeleccionProponente.currentValue,
        changes.procesoSeleccionProponente.previousValue
      );
      let differenceEliminating = this.differenceDeArray(
        changes.procesoSeleccionProponente.previousValue,
        changes.procesoSeleccionProponente.currentValue
      );

      this.removeProponentes(differenceEliminating);
      this.examineProponentes(differenceAddingProponente);
    } else {
      this.examineProponentes(this.procesoSeleccionProponente);
      this.addressForm.markAllAsTouched();
    }
    this.addNewProponenteForm();
    this.removeRemainingProponenteForm();
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    if (value != '') {
      let filtroportipo: string[] = [];
      this.listaProponentesNombres.forEach(element => {
        if (element.tipoProponenteCodigo == this.tipoProponente.value.codigo && element.nombreProponente) {
          if (!filtroportipo.includes(element.nombreProponente)) {
            filtroportipo.push(element.nombreProponente);
          }
        }
      });
      let ret = filtroportipo.filter(x => x.toLowerCase().indexOf(filterValue) === 0);
      return ret;
    } else {
      return [];
    }
  }

  private _filter2(value: string): string[] {
    const filterValue = value.toLowerCase();
    if (value != '') {
      let filtroportipo: string[] = [];
      this.listaProponentesNombres.forEach(element => {
        if (element.tipoProponenteCodigo == this.tipoProponente.value.codigo && element.nombreRepresentanteLegal) {
          if (!filtroportipo.includes(element.nombreRepresentanteLegal)) {
            filtroportipo.push(element.nombreRepresentanteLegal);
          }
        }
      });
      let ret = filtroportipo.filter(x => x.toLowerCase().indexOf(filterValue) === 0);
      return ret;
    } else {
      return [];
    }
  }

  seleccionAutocomplete(nombre: string) {
    let lista: any[] = [];
    this.listaProponentesNombres.forEach(element => {
      if (element.nombreProponente) {
        lista.push(element);
      }
    });

    let ret = lista.filter(x => x.nombreProponente.toLowerCase() === nombre.toLowerCase());
    this.setValueAutocomplete(ret[0]);
  }

  seleccionAutocomplete2(nombre: string) {
    let lista: any[] = [];
    this.listaProponentesNombres.forEach(element => {
      if (element.nombreRepresentanteLegal) {
        lista.push(element);
      }
    });

    let ret = lista.filter(x => x.nombreRepresentanteLegal.toLowerCase() === nombre.toLowerCase());
    this.setValueAutocomplete(ret[0]);
  }

  private _normalizeValue(value: string): string {
    return value.toLowerCase().replace(/\s/g, '');
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  changeDepartamento(i: string | number) {
    let idDepartamento: string;

    // console.log(this.addressForm.get('proponentes').value[i].nombreDepartamento);
    idDepartamento = this.addressForm.get('proponentes').value[i].nombreDepartamento;
    // switch (this.tipoProponente.value.codigo) {
    //   case '1':
    //     idDepartamento = this.personaNaturalForm.get('depaetamento').value.localizacionId;
    //     break;
    //   case '2':
    //     idDepartamento = this.personaJuridicaIndividualForm.get('depaetamento').value.localizacionId;
    //     break;
    //   case '4':
    //     idDepartamento = this.unionTemporalForm.get('depaetamento').value.localizacionId;
    //     break;
    // }

    this.commonService.listaMunicipiosByIdDepartamento(idDepartamento).subscribe(listMun => {
      this.listaMunicipios[i] = listMun;
    });
  }

  private declararSelect() {
    this.tipoProponente = new FormControl('', [Validators.required]);
  }

  CambioNumeroCotizantes(i: string | number) {
    const cuantasEntidades = this.proponentesField.controls[i].get('cuantasEntidades');
    const entidadesField = this.getEntidades(i);

    if (cuantasEntidades.value != '' && cuantasEntidades.value != 0 && cuantasEntidades.value != null) {
      if (cuantasEntidades.value > entidadesField.controls.length && cuantasEntidades.value < 100) {
        while (entidadesField.controls.length < cuantasEntidades.value) {
          entidadesField.push(this.createIntegrante());
        }
      } else if (cuantasEntidades.value <= entidadesField.controls.length && cuantasEntidades.value >= 0) {
        //valido si tiene data
        let bitestavacio = true;
        entidadesField.controls.forEach(element => {
          if (element.get('nombre') != null || element.get('porcentaje') != null) {
            bitestavacio = false;
          }
        });
        if (bitestavacio) {
          while (entidadesField.controls.length > cuantasEntidades.value) {
            this.borrarArray(entidadesField, entidadesField.controls.length - 1);
          }
          cuantasEntidades.setValue(entidadesField.controls.length);
        } else {
          this.openDialog(
            '',
            '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
          );
          cuantasEntidades.setValue(entidadesField.controls.length);
        }
      }
    } else {
      if (cuantasEntidades.value == 0) {
        this.openDialog('', '<b>La cantidad de entidades no puede ser igual a 0.</b>');
        return;
      }
    }
  }

  createIntegrante(): FormGroup {
    return this.fb.group({
      procesoSeleccionIntegranteId: [null],
      nombreIntegrante: [
        null,
        Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(100)])
      ],
      porcentajeParticipacion: [null, Validators.compose([Validators.required, Validators.min(1), Validators.max(100)])]
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
    this.cuantasEntidades.setValue(borrarForm.length);
  }

  onSubmitPersonaNatural() {
    // console.log(this.personaNaturalForm);
    if (this.personaNaturalForm.invalid) {
      this.estaEditando = true;
      this.personaNaturalForm.markAllAsTouched();
      this.openDialog('', '<b>Por favor diligencie completamente el formulario</b>');
      return false;
    }

    if (!this.noTanNuevo) {
      this.procesoSeleccion.procesoSeleccionProponente = [];
    }

    let proponente: ProcesoSeleccionProponente = {
      procesoSeleccionProponenteId: this.personaNaturalForm.get('procesoSeleccionProponenteId').value,
      direccionProponente: this.personaNaturalForm.get('direccion').value,
      emailProponente: this.personaNaturalForm.get('correoElectronico').value,
      localizacionIdMunicipio: this.personaNaturalForm.get('municipio').value
        ? this.personaNaturalForm.get('municipio').value.localizacionId
        : null,
      nombreProponente: this.personaNaturalForm.get('nombre').value
        ? this.personaNaturalForm.get('nombre').value
        : this.myControl.value,
      numeroIdentificacion: this.personaNaturalForm.get('numeroIdentificacion').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      telefonoProponente: this.personaNaturalForm.get('telefono').value,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null
      //tipoIdentificacionCodigo:
    };

    this.procesoSeleccion.procesoSeleccionProponente.push(proponente);
    //console.log(this.personaNaturalForm.value);
  }

  onSubmitPersonaJuridicaIndividual() {
    if (this.personaJuridicaIndividualForm.invalid) {
      this.estaEditando = true;
      this.personaJuridicaIndividualForm.markAllAsTouched();
      this.openDialog('', '<b>Por favor diligencie completamente el formulario</b>');
      return false;
    }
    this.estaEditando = true;
    this.personaJuridicaIndividualForm.markAllAsTouched();
    if (!this.noTanNuevo) {
      this.procesoSeleccion.procesoSeleccionProponente = [];
    }
    let proponente: ProcesoSeleccionProponente = {
      procesoSeleccionProponenteId: this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,

      nombreProponente: this.personaJuridicaIndividualForm.get('nombre').value
        ? this.personaJuridicaIndividualForm.get('nombre').value
        : this.myJuridica.value,
      numeroIdentificacion: this.personaJuridicaIndividualForm.get('numeroIdentificacion').value,
      nombreRepresentanteLegal: this.personaJuridicaIndividualForm.get('representanteLegal').value,
      cedulaRepresentanteLegal: this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').value,
      localizacionIdMunicipio: this.personaJuridicaIndividualForm.get('municipio').value
        ? this.personaJuridicaIndividualForm.get('municipio').value.localizacionId
        : null,
      direccionProponente: this.personaJuridicaIndividualForm.get('direccion').value,
      telefonoProponente: this.personaJuridicaIndividualForm.get('telefono').value,
      emailProponente: this.personaJuridicaIndividualForm.get('correoElectronico').value
    };

    this.procesoSeleccion.procesoSeleccionProponente.push(proponente);

    //console.log(this.personaNaturalForm.value);
  }

  onSubmitUnionTemporal() {
    // console.log(this.unionTemporalForm);
    if (
      this.unionTemporalForm.get('nombreConsorcio').value == '' ||
      this.unionTemporalForm.get('telefono').value == '' ||
      this.unionTemporalForm.get('cedulaRepresentanteLegal').value == '' ||
      this.unionTemporalForm.get('municipio').value == '' ||
      this.unionTemporalForm.get('direccion').value == '' ||
      this.unionTemporalForm.get('correoElectronico').value == '' ||
      this.unionTemporalForm.get('nombreConsorcio').value == null ||
      this.unionTemporalForm.get('telefono').value == null ||
      this.unionTemporalForm.get('cedulaRepresentanteLegal').value == null ||
      this.unionTemporalForm.get('municipio').value == null ||
      this.unionTemporalForm.get('direccion').value == null ||
      this.unionTemporalForm.get('correoElectronico').value == null
    ) {
      this.estaEditando = true;
      this.unionTemporalForm.markAllAsTouched();
      this.openDialog('', '<b>Por favor diligencie completamente el formulario</b>');
      return false;
    }
    this.estaEditando = true;
    this.unionTemporalForm.markAllAsTouched();
    let porcentaje: number = 0;

    if (!this.noTanNuevo) {
      this.procesoSeleccion.procesoSeleccionProponente = [];
    }
    this.procesoSeleccion.procesoSeleccionIntegrante = [];

    let listaIntegrantes = this.unionTemporalForm.get('procesoSeleccionIntegrante') as FormArray;

    let proponente: ProcesoSeleccionProponente = {
      procesoSeleccionProponenteId: this.unionTemporalForm.get('procesoSeleccionProponenteId').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,

      nombreProponente: this.unionTemporalForm.get('nombreConsorcio').value,
      numeroIdentificacion: this.unionTemporalForm.get('numeroIdentificacion').value,
      nombreRepresentanteLegal: this.myJuridica2.value, //this.unionTemporalForm.get('nombre').value,
      cedulaRepresentanteLegal: this.unionTemporalForm.get('cedulaRepresentanteLegal').value,
      localizacionIdMunicipio: this.unionTemporalForm.get('municipio').value
        ? this.unionTemporalForm.get('municipio').value.localizacionId
        : null,
      direccionProponente: this.unionTemporalForm.get('direccion').value,
      telefonoProponente: this.unionTemporalForm.get('telefono').value,
      emailProponente: this.unionTemporalForm.get('correoElectronico').value
    };

    listaIntegrantes.controls.forEach(control => {
      let integrante: ProcesoSeleccionIntegrante = {
        nombreIntegrante: control.get('nombre').value,
        porcentajeParticipacion: control.get('porcentaje').value,
        procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
        procesoSeleccionIntegranteId: control.get('procesoSeleccionIntegranteId').value
      };

      porcentaje = porcentaje + integrante.porcentajeParticipacion;

      this.procesoSeleccion.procesoSeleccionIntegrante.push(integrante);
    });

    let mensajeValidaciones = this.validacionesUnionTemporal(porcentaje);
    if (mensajeValidaciones.length > 0) {
      this.openDialog('', `<b>${mensajeValidaciones}</b>`);
      return false;
    }

    this.procesoSeleccion.procesoSeleccionProponente.push(proponente);
  }

  validacionesUnionTemporal(porcentaje: number): string {
    let mensaje = '';

    if (porcentaje != 100) mensaje = '<b>Los porcentajes de participación no suman 100%.</b>';

    return mensaje;
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  cargarRegistro() {
    this.ngOnInit().then(() => {
      this.procesoSeleccion.procesoSeleccionProponente.forEach(proponente => {
        let tipoProponente = this.listaProponentes.find(p => p.codigo == proponente.tipoProponenteCodigo);
        if (tipoProponente) this.tipoProponente.setValue(tipoProponente);

        let idMunicipio = proponente.localizacionIdMunicipio ? proponente.localizacionIdMunicipio.toString() : '00000';
        let departamentoSeleccionado = this.listaDepartamentos.find(
          d => d.localizacionId == idMunicipio.substring(0, 5)
        );

        this.commonService.listaMunicipiosByIdDepartamento(idMunicipio.substring(0, 5)).subscribe(listMun => {
          this.listaMunicipios = listMun;
          let municipio = listMun.find(m => m.localizacionId == proponente.localizacionIdMunicipio);

          switch (proponente.tipoProponenteCodigo) {
            case '1': {
              this.personaNaturalForm.get('municipio').setValue(municipio);
              this.personaNaturalForm.get('depaetamento').setValue(departamentoSeleccionado);
              this.personaNaturalForm
                .get('procesoSeleccionProponenteId')
                .setValue(proponente.procesoSeleccionProponenteId);
              this.personaNaturalForm.get('direccion').setValue(proponente.direccionProponente);
              this.personaNaturalForm.get('correoElectronico').setValue(proponente.emailProponente);

              //this.personaNaturalForm.get('municipio').setValue( proponente.localizacionIdMunicipio );

              this.personaNaturalForm.get('nombre').setValue(proponente.nombreProponente);
              this.myControl.setValue(proponente.nombreProponente);
              this.personaNaturalForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
              this.personaNaturalForm.get('telefono').setValue(proponente.telefonoProponente);
            }
            case '2': {
              this.personaJuridicaIndividualForm.get('depaetamento').setValue(departamentoSeleccionado);
              this.personaJuridicaIndividualForm
                .get('procesoSeleccionProponenteId')
                .setValue(proponente.procesoSeleccionProponenteId);
              this.personaJuridicaIndividualForm.get('nombre').setValue(proponente.nombreProponente);
              this.myJuridica.setValue(proponente.nombreProponente);
              this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
              this.personaJuridicaIndividualForm
                .get('representanteLegal')
                .setValue(proponente.nombreRepresentanteLegal);
              this.personaJuridicaIndividualForm
                .get('cedulaRepresentanteLegal')
                .setValue(proponente.cedulaRepresentanteLegal);
              this.personaJuridicaIndividualForm.get('municipio').setValue(municipio);
              this.personaJuridicaIndividualForm.get('direccion').setValue(proponente.direccionProponente);
              this.personaJuridicaIndividualForm.get('telefono').setValue(proponente.telefonoProponente);
              this.personaJuridicaIndividualForm.get('correoElectronico').setValue(proponente.emailProponente);
            }
            case '4': {
              let listaIntegrantes = this.unionTemporalForm.get('procesoSeleccionIntegrante') as FormArray;

              this.unionTemporalForm.get('depaetamento').setValue(departamentoSeleccionado);
              this.unionTemporalForm
                .get('procesoSeleccionProponenteId')
                .setValue(proponente.procesoSeleccionProponenteId),
                this.unionTemporalForm.get('nombreConsorcio').setValue(proponente.nombreProponente);
              this.unionTemporalForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
              this.unionTemporalForm.get('nombre').setValue(proponente.nombreRepresentanteLegal);
              this.myJuridica2.setValue(proponente.nombreRepresentanteLegal);
              this.unionTemporalForm.get('cedulaRepresentanteLegal').setValue(proponente.cedulaRepresentanteLegal);
              this.unionTemporalForm.get('municipio').setValue(municipio);
              this.unionTemporalForm.get('direccion').setValue(proponente.direccionProponente);
              this.unionTemporalForm.get('telefono').setValue(proponente.telefonoProponente);
              this.unionTemporalForm.get('correoElectronico').setValue(proponente.emailProponente);

              this.procesoSeleccion.procesoSeleccionIntegrante.forEach(integrante => {
                let control = this.createIntegrante();
                control.get('nombre').setValue(integrante.nombreIntegrante);
                control.get('porcentaje').setValue(integrante.porcentajeParticipacion);
                control.get('procesoSeleccionIntegranteId').setValue(integrante.procesoSeleccionIntegranteId);

                listaIntegrantes.push(control);
              });

              if (listaIntegrantes.length > 0) {
                this.cuantasEntidades.setValue(listaIntegrantes.length);
              }
            }
          }
        });
      });
    });
  }

  setValueAutocomplete(proponente: any) {
    let idMunicipio = proponente.localizacionIdMunicipio ? proponente.localizacionIdMunicipio.toString() : '00000';
    let departamentoSeleccionado = this.listaDepartamentos.find(d => d.localizacionId == idMunicipio.substring(0, 5));
    this.commonService.listaMunicipiosByIdDepartamento(departamentoSeleccionado.localizacionId).subscribe(listMun => {
      this.listaMunicipios = listMun;
      let municipio = this.listaMunicipios.find(m => m.localizacionId == proponente.localizacionIdMunicipio);
      switch (proponente.tipoProponenteCodigo) {
        case '1': {
          this.personaNaturalForm.get('municipio').setValue(municipio);
          this.personaNaturalForm.get('depaetamento').setValue(departamentoSeleccionado);
          this.personaNaturalForm.get('procesoSeleccionProponenteId').setValue(0);
          this.personaNaturalForm.get('direccion').setValue(proponente.direccionProponente);
          this.personaNaturalForm.get('correoElectronico').setValue(proponente.emailProponente);

          //this.personaNaturalForm.get('municipio').setValue( proponente.localizacionIdMunicipio );

          this.personaNaturalForm.get('nombre').setValue(this.myControl.value);
          this.personaNaturalForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
          this.personaNaturalForm.get('telefono').setValue(proponente.telefonoProponente);
        }
        case '2': {
          this.personaJuridicaIndividualForm.get('depaetamento').setValue(departamentoSeleccionado);
          this.personaJuridicaIndividualForm.get('municipio').setValue(municipio);
          this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').setValue(0);
          this.personaJuridicaIndividualForm.get('nombre').setValue(this.myJuridica.value);
          this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
          this.personaJuridicaIndividualForm.get('representanteLegal').setValue(proponente.nombreRepresentanteLegal);
          this.personaJuridicaIndividualForm
            .get('cedulaRepresentanteLegal')
            .setValue(proponente.cedulaRepresentanteLegal);

          this.personaJuridicaIndividualForm.get('direccion').setValue(proponente.direccionProponente);
          this.personaJuridicaIndividualForm.get('telefono').setValue(proponente.telefonoProponente);
          this.personaJuridicaIndividualForm.get('correoElectronico').setValue(proponente.emailProponente);
        }
        case '4': {
          let listaIntegrantes = this.unionTemporalForm.get('procesoSeleccionIntegrante') as FormArray;

          this.unionTemporalForm.get('depaetamento').setValue(departamentoSeleccionado);
          this.unionTemporalForm.get('procesoSeleccionProponenteId').setValue(0),
            this.unionTemporalForm.get('nombreConsorcio').setValue(proponente.nombreProponente);
          this.unionTemporalForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
          this.unionTemporalForm.get('nombre').setValue(proponente.nombreRepresentanteLegal);
          this.unionTemporalForm.get('cedulaRepresentanteLegal').setValue(proponente.cedulaRepresentanteLegal);
          this.unionTemporalForm.get('municipio').setValue(municipio);
          this.unionTemporalForm.get('direccion').setValue(proponente.direccionProponente);
          this.unionTemporalForm.get('telefono').setValue(proponente.telefonoProponente);
          this.unionTemporalForm.get('correoElectronico').setValue(proponente.emailProponente);

          /*this.procesoSeleccion.procesoSeleccionIntegrante.forEach( integrante => {
            let control = this.createIntegrante();
            control.get('nombre').setValue( integrante.nombreIntegrante );
            control.get('porcentaje').setValue( integrante.porcentajeParticipacion );
            control.get('procesoSeleccionIntegranteId').setValue( integrante.procesoSeleccionIntegranteId );
  
            listaIntegrantes.push( control );  
  
          })
  
          console.log( this.procesoSeleccion );
          this.cuantasEntidades.setValue( listaIntegrantes.length ); */
        }
      }
    });
  }
  limpiarForms() {
    this.myControl.setValue('');
    this.personaNaturalForm.get('municipio').setValue('');
    this.personaNaturalForm.get('depaetamento').setValue('');
    this.personaNaturalForm.get('procesoSeleccionProponenteId').setValue('');
    this.personaNaturalForm.get('direccion').setValue('');
    this.personaNaturalForm.get('correoElectronico').setValue('');
    this.personaNaturalForm.get('nombre').setValue('');
    this.personaNaturalForm.get('numeroIdentificacion').setValue('');
    this.personaNaturalForm.get('telefono').setValue('');

    this.myJuridica.setValue('');
    this.personaJuridicaIndividualForm.get('depaetamento').setValue('');
    this.personaJuridicaIndividualForm.get('municipio').setValue('');
    this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').setValue('');
    this.personaJuridicaIndividualForm.get('nombre').setValue('');
    this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue('');
    this.personaJuridicaIndividualForm.get('representanteLegal').setValue('');
    this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').setValue('');

    this.personaJuridicaIndividualForm.get('direccion').setValue('');
    this.personaJuridicaIndividualForm.get('telefono').setValue('');
    this.personaJuridicaIndividualForm.get('correoElectronico').setValue('');

    let listaIntegrantes = this.unionTemporalForm.get('procesoSeleccionIntegrante') as FormArray;

    this.unionTemporalForm.get('depaetamento').setValue('');
    this.unionTemporalForm.get('procesoSeleccionProponenteId').setValue(''),
      this.unionTemporalForm.get('nombreConsorcio').setValue('');
    this.unionTemporalForm.get('numeroIdentificacion').setValue('');
    this.unionTemporalForm.get('nombre').setValue('');
    this.unionTemporalForm.get('cedulaRepresentanteLegal').setValue('');
    this.unionTemporalForm.get('municipio').setValue('');
    this.unionTemporalForm.get('direccion').setValue('');
    this.unionTemporalForm.get('telefono').setValue('');
    this.unionTemporalForm.get('correoElectronico').setValue('');
  }

  private differenceDeArray(arr1: any[], arr2: any[]) {
    return arr1.filter(elemento => arr2.indexOf(elemento) == -1);
  }

  removeProponentes(differenceEliminating: any[]) {
    for (let proponente of differenceEliminating) {
      if (proponente.tipoProponenteCodigo === '1') {
        // console.log(this.proponentesField);
        for (let i = 0; i < this.proponentesField.controls.length; i++) {
          const element = this.proponentesField.controls[i].value.numeroIdentificacion;
          if (element === proponente.numeroIdentificacion) {
            this.borrarArray(this.proponentesField, i);
          }
        }
      }
    }
  }

  examineProponentes(differenceAddingProponente: any[]) {
    let index = 0;
    for (let proponente of differenceAddingProponente) {
      console.log(proponente);
      // if (proponente.tipoProponenteCodigo === '1') {
      this.proponentesField.push(
        this.fb.group({
          ProcesoSeleccionProponenteId: [
            proponente.procesoSeleccionProponenteId ? proponente.procesoSeleccionProponenteId : 0
          ],
          tipoProponenteCodigo: [
            proponente.tipoProponenteCodigo ? proponente.tipoProponenteCodigo : null,
            Validators.required
          ],
          nombreProponente: [
            proponente.nombreProponente ? proponente.nombreProponente : null,
            Validators.compose([Validators.minLength(2), Validators.maxLength(1000)])
          ],
          numeroIdentificacion: [
            proponente.numeroIdentificacion ? proponente.numeroIdentificacion : null,
            Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(12)])
          ],
          nombreDepartamento: [
            proponente.departamento ? proponente.departamento.localizacionId : null,
            Validators.required
          ],
          localizacionIdMunicipio: [
            proponente.localizacionIdMunicipio ? proponente.localizacionIdMunicipio : null,
            Validators.required
          ],
          direccionProponente: [
            proponente.direccionProponente ? proponente.direccionProponente : null,
            Validators.compose([Validators.required, Validators.maxLength(500)])
          ],
          telefonoProponente: [
            proponente.telefonoProponente ? proponente.telefonoProponente : null,
            Validators.compose([Validators.required, Validators.minLength(7), Validators.maxLength(10)])
          ],
          emailProponente: [
            proponente.emailProponente ? proponente.emailProponente : null,
            Validators.compose([
              Validators.required,
              Validators.minLength(10),
              Validators.maxLength(1000),
              Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
            ])
          ],
          procesoSeleccionId: [
            proponente.procesoSeleccionId ? proponente.procesoSeleccionId : null,
            Validators.required
          ],
          nombreRepresentanteLegal: [
            proponente.nombreRepresentanteLegal ? proponente.nombreRepresentanteLegal : null,
            Validators.required
          ],
          cedulaRepresentanteLegal: [
            proponente.cedulaRepresentanteLegal ? proponente.cedulaRepresentanteLegal : null,
            Validators.required
          ],
          cuantasEntidades: [proponente.procesoSeleccionIntegrante.length ? proponente.procesoSeleccionIntegrante.length : null, Validators.required],
          procesoSeleccionIntegrante: this.fb.array([]),
          tipoIdentificacionCodigo: [
            proponente.tipoIdentificacionCodigo ? proponente.tipoIdentificacionCodigo : null,
            Validators.required
          ]
        })
      );
      for (let element of proponente.procesoSeleccionIntegrante.length) {
        this.getEntidades(index).push(
          this.fb.group({
            procesoSeleccionIntegranteId: [
              element.procesoSeleccionIntegranteId ? element.procesoSeleccionIntegranteId : null
            ],
            nombreIntegrante: [
              element.nombreIntegrante ? element.nombreIntegrante : null,
              Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(100)])
            ],
            porcentajeParticipacion: [
              element.porcentajeParticipacion ? element.porcentajeParticipacion : null,
              Validators.compose([Validators.required, Validators.min(1), Validators.max(100)])
            ]
          })
        );
      }
      this.changeDepartamento(index);
      index++;
      // }
    }
  }

  addNewProponenteForm() {
    let amountNuevos = this.amountNuevosProponentes - this.procesoSeleccionProponente.length + 1;
    if (this.procesoSeleccionProponente.includes('Nuevo')) {
      for (let index = 0; index < amountNuevos; index++) {
        this.agregarProponentes();
      }
    }
  }

  removeRemainingProponenteForm() {
    let i = 0;
    while (this.proponentesField.controls.length > this.amountNuevosProponentes) {
      const element = this.proponentesField.controls[i].value.numeroIdentificacion;
      if (element === null || element === undefined || element === '') {
        this.borrarArray(this.proponentesField, i);
      }
      i++;
    }
  }

  guardarNuevosProponentes() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.personaJuridicaIndividualForm.markAllAsTouched();

    for (let proponente of this.addressForm.get('proponentes').value) {
      this.procesoSeleccion.procesoSeleccionProponente.push(proponente);
    }

    this.guardar.emit(null);
  }
}
