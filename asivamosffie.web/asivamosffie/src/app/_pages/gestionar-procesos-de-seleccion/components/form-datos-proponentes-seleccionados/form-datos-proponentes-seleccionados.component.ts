import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormControl, FormArray, FormGroup } from '@angular/forms';
import { ProcesoSeleccion, ProcesoSeleccionProponente, ProcesoSeleccionIntegrante, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { Dominio, Localizacion, CommonService } from 'src/app/core/_services/common/common.service';
import { forkJoin, Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-form-datos-proponentes-seleccionados',
  templateUrl: './form-datos-proponentes-seleccionados.component.html',
  styleUrls: ['./form-datos-proponentes-seleccionados.component.scss']
})
export class FormDatosProponentesSeleccionadosComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Input() editar:boolean;
  @Output() guardar: EventEmitter<any> = new EventEmitter();

  listaDepartamentos: Localizacion[] = [];
  listaMunicipios: Localizacion[] = [];
  listaProponentes: Dominio[] = [];
  tipoProponente: FormControl;
  myControl = new FormControl();
  myJuridica = new FormControl();
  myJuridica2 = new FormControl();
  representanteLegal = new FormControl();

  personaNaturalForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
    ],
    numeroIdentificacion: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([
      Validators.required, Validators.maxLength(500)])
    ],
    telefono: [null, Validators.compose([
      Validators.required, Validators.minLength(7), Validators.maxLength(10)])
    ],
    correoElectronico: [null, Validators.compose([
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(1000),
      // Validators.email,
      Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
    ])]
  });

  personaJuridicaIndividualForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(1000)])
    ],
    numeroIdentificacion: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    representanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
    ],
    cedulaRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([
      Validators.required, Validators.maxLength(500)])
    ],
    telefono: [null, Validators.compose([
      Validators.required, Validators.minLength(7), Validators.maxLength(10)])
    ],
    correoElectronico: [null, Validators.compose([
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(1000),
      // Validators.email,
      Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
    ])]
  });

  unionTemporalForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    cuantasEntidades: [null, Validators.compose([
      Validators.required,])
    ],
    nombreConsorcio: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(1000)])
    ],
    entidades: this.fb.array([]),
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
    ],
    numeroIdentificacion: [null, Validators.compose([
      Validators.required, Validators.maxLength(12)])
    ],
    cedulaRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.maxLength(12)])
    ],
    depaetamento: [null, Validators.required],
    municipio: [null, Validators.required],
    direccion: [null, Validators.compose([
      Validators.required, Validators.maxLength(500)])
    ],
    telefono: [null, Validators.compose([
      Validators.required, Validators.minLength(7), Validators.maxLength(10)])
    ],
    correoElectronico: [null, Validators.compose([
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(1000),
      // Validators.email,
      Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
    ])]
  });
  listaProponentesNombres: any[] = [];
  nombresapo: string[] = [];
  filteredNameJuridica2: Observable<string[]>;

  get entidades() {
    return this.unionTemporalForm.get('entidades') as FormArray;
  }
  filteredName: Observable<string[]>;
  filteredNameJuridica: Observable<string[]>;
  filteredNameJuridicaName: Observable<string[]>;

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    public dialog: MatDialog,
    private procesoSeleccionService: ProcesoSeleccionService
  ) {
    this.declararSelect();
  }

  ngOnDestroy(): void {
    if ( this.personaNaturalForm.dirty) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSubmitPersonaNatural();          
        }           
      });
    }

    if ( this.personaJuridicaIndividualForm.dirty) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSubmitPersonaJuridicaIndividual();          
        }           
      });
    }

    if ( this.unionTemporalForm.dirty) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSubmitUnionTemporal();          
        }           
      });
    }
  };

  ngOnInit() {

    return new Promise(resolve => {
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
        this.filteredNameJuridicaName = this.representanteLegal.valueChanges.pipe(
          startWith(''),
          map(value => this._filter3(value))
        );


        resolve();
      })
    })

  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    if (value != "") {
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
    }
    else {
      return [];
    }

  }

  private _filter2(value: string): string[] {
    const filterValue = value.toLowerCase();
    if (value != "") {
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
    }
    else {
      return [];
    }

  }
  private _filter3(value: string): string[] {
    const filterValue = value.toLowerCase();
    if (value != "") {
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
    }
    else {
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

  seleccionAutocomplete3(nombre: string) {
    let lista: any[] = [];
    this.listaProponentesNombres.forEach(element => {
      if (element.nombreProponente) {
        lista.push(element);
      }
    });

    let ret = lista.filter(x => x.nombreProponente.toLowerCase() === nombre.toLowerCase());
    this.setValueAutocomplete(ret[0]);
  }


  private _normalizeValue(value: string): string {
    return value.toLowerCase().replace(/\s/g, '');
  }

  openDialog(modalTitle: string, modalText: string,refresh:boolean=false) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(refresh)
      {
        location.reload();
      }
      
    });
  }

  changeDepartamento() {
    let idDepartamento: string;

    switch (this.tipoProponente.value.codigo) {
      case '1': idDepartamento = this.personaNaturalForm.get('depaetamento').value.localizacionId; break;
      case '2': idDepartamento = this.personaJuridicaIndividualForm.get('depaetamento').value.localizacionId; break;
      case '4': idDepartamento = this.unionTemporalForm.get('depaetamento').value.localizacionId; break;
    }

    this.commonService.listaMunicipiosByIdDepartamento(idDepartamento).subscribe(listMun => {
      this.listaMunicipios = listMun;
    })
  }

  private declararSelect() {
    this.tipoProponente = new FormControl('', [Validators.required]);
  }

  CambioNumeroCotizantes() {
    const formIntegrantes = this.unionTemporalForm.value;
    if (formIntegrantes.cuantasEntidades != '' && formIntegrantes.cuantasEntidades != 0 && formIntegrantes.cuantasEntidades != null) {
      if (formIntegrantes.cuantasEntidades > this.entidades.length && formIntegrantes.cuantasEntidades < 100) {
        while (this.entidades.length < formIntegrantes.cuantasEntidades) {
          this.entidades.push(this.createIntegrante());
        }
      } else if (formIntegrantes.cuantasEntidades <= this.entidades.length && formIntegrantes.cuantasEntidades >= 0) {
        //valido si tiene data
        let bitestavacio = true;
        formIntegrantes.entidades.forEach(element => {
          if (element.nombre != null || element.porcentaje != null) {
            bitestavacio = false
          }
        });
        if (bitestavacio) {
          while (this.entidades.length > formIntegrantes.cuantasEntidades) {
            this.borrarArray(this.entidades, this.entidades.length - 1);
          }
          this.unionTemporalForm.get("cuantasEntidades").setValue(this.entidades.length);
        }
        else {
          this.openDialog("", "<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>");
          this.unionTemporalForm.get("cuantasEntidades").setValue(this.entidades.length);
        }

      }
    }
    else {
      if (formIntegrantes.cuantasEntidades == 0) {

        this.openDialog("", "<b>La cantidad de entidades no puede ser igual a 0.</b>");
        return;

      }
    }

  }

  createIntegrante(): FormGroup {
    return this.fb.group({
      procesoSeleccionIntegranteId: [],
      nombre: [null, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(1000)])
      ],
      porcentaje: [null, Validators.compose([
        Validators.required, Validators.min(1), Validators.max(100), Validators.maxLength(2)])
      ]
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
    this.unionTemporalForm.get("cuantasEntidades").setValue(borrarForm.length);
  }

  borrarIntegrante(borrarForm: any, i: number) {
    if(borrarForm.value[i].procesoSeleccionIntegranteId>0)
    {
      this.procesoSeleccionService.deleteProcesoSeleccionIntegranteByID(borrarForm.value[i].procesoSeleccionIntegranteId).subscribe(
        respuesta=>
        {
          this.openDialog("","<b>La información ha sido eliminada correctamente.</b>",true);
        }
      );
    }
    console.log(borrarForm);
    /*borrarForm.removeAt(i);
    this.unionTemporalForm.get("cuantasEntidades").setValue(borrarForm.length);*/
  }

  onSubmitPersonaNatural() {

    this.procesoSeleccion.procesoSeleccionProponente = [];
    let proponente: ProcesoSeleccionProponente = {
      procesoSeleccionProponenteId: this.personaNaturalForm.get('procesoSeleccionProponenteId').value,
      direccionProponente: this.personaNaturalForm.get('direccion').value,
      emailProponente: this.personaNaturalForm.get('correoElectronico').value,
      localizacionIdMunicipio: this.personaNaturalForm.get('municipio').value ? this.personaNaturalForm.get('municipio').value.localizacionId : null,
      nombreProponente: this.personaNaturalForm.get('nombre').value ? this.personaNaturalForm.get('nombre').value : this.myControl.value,
      numeroIdentificacion: this.personaNaturalForm.get('numeroIdentificacion').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      telefonoProponente: this.personaNaturalForm.get('telefono').value,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,
      //tipoIdentificacionCodigo: 
    }

    this.procesoSeleccion.procesoSeleccionProponente.push(proponente);

    this.guardar.emit(null);
    //console.log(this.personaNaturalForm.value);
  }

  onSubmitPersonaJuridicaIndividual() {

    this.procesoSeleccion.procesoSeleccionProponente = [];
    let proponente: ProcesoSeleccionProponente = {

      procesoSeleccionProponenteId: this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,

      nombreProponente: this.personaJuridicaIndividualForm.get('nombre').value ? this.personaJuridicaIndividualForm.get('nombre').value : this.myJuridica.value,
      numeroIdentificacion: this.personaJuridicaIndividualForm.get('numeroIdentificacion').value,
      nombreRepresentanteLegal: this.personaJuridicaIndividualForm.get('representanteLegal').value?this.personaJuridicaIndividualForm.get('representanteLegal').value:this.representanteLegal.value,
      cedulaRepresentanteLegal: this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').value,
      localizacionIdMunicipio: this.personaJuridicaIndividualForm.get('municipio').value ? this.personaJuridicaIndividualForm.get('municipio').value.localizacionId : null,
      direccionProponente: this.personaJuridicaIndividualForm.get('direccion').value,
      telefonoProponente: this.personaJuridicaIndividualForm.get('telefono').value,
      emailProponente: this.personaJuridicaIndividualForm.get('correoElectronico').value,

    }

    this.procesoSeleccion.procesoSeleccionProponente.push(proponente);

    this.guardar.emit(null);
    //console.log(this.personaNaturalForm.value);
  }

  validacionesUnionTemporal(porcentaje: number): string {
    let mensaje = '';

    if (porcentaje != 100)
      mensaje = '<b>Los porcentajes de participación no suman 100%.</b>'

    return mensaje;
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  onSubmitUnionTemporal() {

    let porcentaje: number = 0;

    this.procesoSeleccion.procesoSeleccionProponente = [];
    this.procesoSeleccion.procesoSeleccionIntegrante = [];

    let listaIntegrantes = this.unionTemporalForm.get('entidades') as FormArray;

    let proponente: ProcesoSeleccionProponente = {

      procesoSeleccionProponenteId: this.unionTemporalForm.get('procesoSeleccionProponenteId').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,

      nombreProponente: this.unionTemporalForm.get('nombreConsorcio').value,
      numeroIdentificacion: this.unionTemporalForm.get('numeroIdentificacion').value,
      nombreRepresentanteLegal: this.myJuridica2.value,//this.unionTemporalForm.get('nombre').value,
      cedulaRepresentanteLegal: this.unionTemporalForm.get('cedulaRepresentanteLegal').value,
      localizacionIdMunicipio: this.unionTemporalForm.get('municipio').value ? this.unionTemporalForm.get('municipio').value.localizacionId : null,
      direccionProponente: this.unionTemporalForm.get('direccion').value,
      telefonoProponente: this.unionTemporalForm.get('telefono').value,
      emailProponente: this.unionTemporalForm.get('correoElectronico').value,

    }

    listaIntegrantes.controls.forEach(control => {
      let integrante: ProcesoSeleccionIntegrante = {
        nombreIntegrante: control.get('nombre').value,
        porcentajeParticipacion: control.get('porcentaje').value,
        procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
        procesoSeleccionIntegranteId: control.get('procesoSeleccionIntegranteId').value,

      }

      porcentaje = porcentaje + integrante.porcentajeParticipacion;

      this.procesoSeleccion.procesoSeleccionIntegrante.push(integrante);
    })

    let mensajeValidaciones = this.validacionesUnionTemporal(porcentaje);
    if (mensajeValidaciones.length > 0) {
      this.openDialog('', `<b>${mensajeValidaciones}</b>`);
      return false;
    }

    this.procesoSeleccion.procesoSeleccionProponente.push(proponente);

    this.guardar.emit(null);
  }

  cargarRegistro() {

    this.ngOnInit().then(() => {
      
      this.procesoSeleccion.procesoSeleccionProponente.forEach(proponente => {
        let tipoProponente = this.listaProponentes.find(p => p.codigo == proponente.tipoProponenteCodigo)
        if (tipoProponente) this.tipoProponente.setValue(tipoProponente);

        let idMunicipio = proponente.localizacionIdMunicipio ? proponente.localizacionIdMunicipio.toString() : "00000";
        let departamentoSeleccionado = this.listaDepartamentos.find(d => d.localizacionId == idMunicipio.substring(0, 5));


        this.commonService.listaMunicipiosByIdDepartamento(idMunicipio.substring(0, 5)).subscribe(listMun => {
          this.listaMunicipios = listMun;
          let municipio = listMun.find(m => m.localizacionId == proponente.localizacionIdMunicipio)

          switch (proponente.tipoProponenteCodigo) {
            case "1": {

              this.personaNaturalForm.get('municipio').setValue(municipio);
              this.personaNaturalForm.get('depaetamento').setValue(departamentoSeleccionado);
              this.personaNaturalForm.get('procesoSeleccionProponenteId').setValue(proponente.procesoSeleccionProponenteId);
              this.personaNaturalForm.get('direccion').setValue(proponente.direccionProponente);
              this.personaNaturalForm.get('correoElectronico').setValue(proponente.emailProponente);

              //this.personaNaturalForm.get('municipio').setValue( proponente.localizacionIdMunicipio );

              this.personaNaturalForm.get('nombre').setValue(proponente.nombreProponente);
              this.myControl.setValue(proponente.nombreProponente);
              this.personaNaturalForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
              this.personaNaturalForm.get('telefono').setValue(proponente.telefonoProponente);

            }
            case "2": {
              this.personaJuridicaIndividualForm.get('depaetamento').setValue(departamentoSeleccionado);
              this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').setValue(proponente.procesoSeleccionProponenteId);
              this.personaJuridicaIndividualForm.get('nombre').setValue(proponente.nombreProponente);
              this.myJuridica.setValue(proponente.nombreProponente);
              this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
              this.personaJuridicaIndividualForm.get('representanteLegal').setValue(proponente.nombreRepresentanteLegal);
              this.representanteLegal.setValue(proponente.nombreRepresentanteLegal);
              this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').setValue(proponente.cedulaRepresentanteLegal);
              this.personaJuridicaIndividualForm.get('municipio').setValue(municipio);
              this.personaJuridicaIndividualForm.get('direccion').setValue(proponente.direccionProponente);
              this.personaJuridicaIndividualForm.get('telefono').setValue(proponente.telefonoProponente);
              this.personaJuridicaIndividualForm.get('correoElectronico').setValue(proponente.emailProponente);
            }
            case "4": {
              (<FormArray>this.unionTemporalForm.get('entidades')).clear();
              let listaIntegrantes = this.unionTemporalForm.get('entidades') as FormArray;

              this.unionTemporalForm.get('depaetamento').setValue(departamentoSeleccionado);
              this.unionTemporalForm.get('procesoSeleccionProponenteId').setValue(proponente.procesoSeleccionProponenteId),
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

              })

              if (listaIntegrantes.length > 0) {
                this.unionTemporalForm.get('cuantasEntidades').setValue(listaIntegrantes.length);
              }

            }
          }
        })
      })
    });
  }

  setValueAutocomplete(proponente: any) {
    let idMunicipio = proponente.localizacionIdMunicipio ? proponente.localizacionIdMunicipio.toString() : "00000";
    let departamentoSeleccionado = this.listaDepartamentos.find(d => d.localizacionId == idMunicipio.substring(0, 5));
    this.commonService.listaMunicipiosByIdDepartamento(departamentoSeleccionado.localizacionId).subscribe(listMun => {
      this.listaMunicipios = listMun;
      let municipio = this.listaMunicipios.find(m => m.localizacionId == proponente.localizacionIdMunicipio)
      switch (proponente.tipoProponenteCodigo) {
        case "1": {

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
        case "2": {
          this.personaJuridicaIndividualForm.get('depaetamento').setValue(departamentoSeleccionado);
          this.personaJuridicaIndividualForm.get('municipio').setValue(municipio);
          this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').setValue(0);
          this.personaJuridicaIndividualForm.get('nombre').setValue(this.myJuridica.value);
          this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue(proponente.numeroIdentificacion);
          this.personaJuridicaIndividualForm.get('representanteLegal').setValue(this.representanteLegal.value);
          this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').setValue(proponente.cedulaRepresentanteLegal);
          if (this.myJuridica.value == "") {
            this.myJuridica.setValue(proponente.nombreProponente);
          }

          //console.log(this.representanteLegal);
          if (this.representanteLegal.value == "") {
            this.representanteLegal.setValue(proponente.representanteLegal);
          }

          this.personaJuridicaIndividualForm.get('direccion').setValue(proponente.direccionProponente);
          this.personaJuridicaIndividualForm.get('telefono').setValue(proponente.telefonoProponente);
          this.personaJuridicaIndividualForm.get('correoElectronico').setValue(proponente.emailProponente);
        }
        case "4": {

          let listaIntegrantes = this.unionTemporalForm.get('entidades') as FormArray;

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
          this.unionTemporalForm.get('cuantasEntidades').setValue( listaIntegrantes.length ); */
        }
      }

    });
  }
  limpiarForms() {
    this.myControl.setValue("");
    this.personaNaturalForm.get('municipio').setValue("");
    this.personaNaturalForm.get('depaetamento').setValue("");
    this.personaNaturalForm.get('procesoSeleccionProponenteId').setValue("");
    this.personaNaturalForm.get('direccion').setValue("");
    this.personaNaturalForm.get('correoElectronico').setValue("");
    this.personaNaturalForm.get('nombre').setValue("");
    this.personaNaturalForm.get('numeroIdentificacion').setValue("");
    this.personaNaturalForm.get('telefono').setValue("");

    this.myJuridica.setValue("");
    this.personaJuridicaIndividualForm.get('depaetamento').setValue("");
    this.personaJuridicaIndividualForm.get('municipio').setValue("");
    this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').setValue("");
    this.personaJuridicaIndividualForm.get('nombre').setValue("");
    this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue("");
    this.personaJuridicaIndividualForm.get('representanteLegal').setValue("");
    this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').setValue("");

    this.personaJuridicaIndividualForm.get('direccion').setValue("");
    this.personaJuridicaIndividualForm.get('telefono').setValue("");
    this.personaJuridicaIndividualForm.get('correoElectronico').setValue("");

    let listaIntegrantes = this.unionTemporalForm.get('entidades') as FormArray;

    this.unionTemporalForm.get('depaetamento').setValue("");
    this.unionTemporalForm.get('procesoSeleccionProponenteId').setValue(""),
      this.unionTemporalForm.get('nombreConsorcio').setValue("");
    this.unionTemporalForm.get('numeroIdentificacion').setValue("");
    this.unionTemporalForm.get('nombre').setValue("");
    this.unionTemporalForm.get('cedulaRepresentanteLegal').setValue("");
    this.unionTemporalForm.get('municipio').setValue("");
    this.unionTemporalForm.get('direccion').setValue("");
    this.unionTemporalForm.get('telefono').setValue("");
    this.unionTemporalForm.get('correoElectronico').setValue("");
  }
}
