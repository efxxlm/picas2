import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormControl, FormArray, FormGroup } from '@angular/forms';
import { ProcesoSeleccion, ProcesoSeleccionProponente, ProcesoSeleccionIntegrante, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { Dominio, Localizacion, CommonService } from 'src/app/core/_services/common/common.service';
import { forkJoin, Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { startWith,map } from 'rxjs/operators';

@Component({
  selector: 'app-form-datos-proponentes-seleccionados',
  templateUrl: './form-datos-proponentes-seleccionados.component.html',
  styleUrls: ['./form-datos-proponentes-seleccionados.component.scss']
})
export class FormDatosProponentesSeleccionadosComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 

  listaDepartamentos: Localizacion[] = [];
  listaMunicipios: Localizacion[] = [];
  listaProponentes: Dominio[] = [];
  tipoProponente: FormControl;
  myControl = new FormControl();
  myJuridica = new FormControl();

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
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    telefono: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    correoElectronico: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ]
  });

  personaJuridicaIndividualForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
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
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    telefono: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    correoElectronico: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ]
  });

  unionTemporalForm = this.fb.group({
    procesoSeleccionProponenteId: [],
    cuantasEntidades: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2)])
    ],
    nombreConsorcio: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
    ],
    entidades: this.fb.array([]),
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(100)])
    ],
    numeroIdentificacion: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    cedulaRepresentanteLegal: [null, Validators.compose([
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
  listaProponentesNombres: any[]=[];
  nombresapo: string[]=[];

  get entidades() {
    return this.unionTemporalForm.get('entidades') as FormArray;
  }
  filteredName: Observable<string[]>;
  filteredNameJuridica: Observable<string[]>;

  constructor(
              private fb: FormBuilder,
              private commonService: CommonService,
              public dialog: MatDialog,    
              private procesoSeleccionService:ProcesoSeleccionService
             ) 
  {
    this.declararSelect();
  }
  ngOnInit() {
    
    return new Promise( resolve => {
      forkJoin([
        
        this.commonService.listaTipoProponente(),
        this.commonService.listaDepartamentos(),
        this.procesoSeleccionService.getProcesoSeleccionProponentes()

      ]).subscribe( respuesta => {
        this.listaProponentes = respuesta[0];
        this.listaDepartamentos = respuesta[1];
        this.listaProponentesNombres =respuesta[2];
        console.log(respuesta[2]);
        respuesta[2].forEach(element => {
          if(element.nombreProponente)
          {
            if(!this.nombresapo.includes(element.nombreProponente))
            {
              this.nombresapo.push(element.nombreProponente);
            }
            
          }
          
        });
        console.log(this.nombresapo);
        this.filteredName = this.myControl.valueChanges.pipe(
          startWith(''),
          map(value => this._filter(value))
        );
        this.filteredNameJuridica = this.myJuridica.valueChanges.pipe(
          startWith(''),
          map(value => this._filter(value))
        );
        
        resolve();
      })
    })

  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();    
console.log(this.tipoProponente.value.codigo);
    if(value!="")
    {      
      let filtroportipo:string[]=[];
      this.listaProponentesNombres.forEach(element => {        
        if(element.tipoProponenteCodigo==this.tipoProponente.value.codigo && element.nombreProponente)
        {
          if(!filtroportipo.includes(element.nombreProponente))
          {
            filtroportipo.push(element.nombreProponente);
          }
        }
      });
      let ret= filtroportipo.filter(x=> x.toLowerCase().indexOf(filterValue) === 0);      
      return ret;
    }
    else
    {
      return this.nombresapo;
    }
    
  }

  seleccionAutocomplete(nombre:string){
    let lista:any[]=[];
    this.listaProponentesNombres.forEach(element => {
      if(element.nombreProponente)
      {
        lista.push(element);
      }      
    });
    
    let ret= lista.filter(x=> x.nombreProponente.toLowerCase() === nombre.toLowerCase());
    this.setValueAutocomplete(ret[0]);    
  }

  private _normalizeValue(value: string): string {
    return value.toLowerCase().replace(/\s/g, '');
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  changeDepartamento(){
    let idDepartamento: string;

    switch( this.tipoProponente.value.codigo ){
      case '1': idDepartamento = this.personaNaturalForm.get('depaetamento').value.localizacionId; break;
      case '2': idDepartamento = this.personaJuridicaIndividualForm.get('depaetamento').value.localizacionId; break;
      case '4': idDepartamento = this.unionTemporalForm.get('depaetamento').value.localizacionId; break;
    }

    this.commonService.listaMunicipiosByIdDepartamento( idDepartamento ).subscribe( listMun => {
      this.listaMunicipios = listMun;
    })
  }

  private declararSelect() {
    this.tipoProponente = new FormControl('', [Validators.required]);
  }

  CambioNumeroCotizantes() {
    const formIntegrantes = this.unionTemporalForm.value;
    if (formIntegrantes.cuantasEntidades > this.entidades.length && formIntegrantes.cuantasEntidades < 100) {
      while (this.entidades.length < formIntegrantes.cuantasEntidades) {
        this.entidades.push( this.createIntegrante() );
      }
    } else if (formIntegrantes.cuantasEntidades <= this.entidades.length && formIntegrantes.cuantasEntidades >= 0) {
      while (this.entidades.length > formIntegrantes.cuantasEntidades) {
        this.borrarArray(this.entidades, this.entidades.length - 1);
      }
    }
  }

  createIntegrante(): FormGroup {
    return this.fb.group({
      procesoSeleccionIntegranteId: [],
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

    this.procesoSeleccion.procesoSeleccionProponente = [];
    let proponente: ProcesoSeleccionProponente = {
      procesoSeleccionProponenteId: this.personaNaturalForm.get('procesoSeleccionProponenteId').value,
      direccionProponente: this.personaNaturalForm.get('direccion').value,
      emailProponente: this.personaNaturalForm.get('correoElectronico').value,
      localizacionIdMunicipio: this.personaNaturalForm.get('municipio').value ? this.personaNaturalForm.get('municipio').value.localizacionId : null,
      nombreProponente: this.personaNaturalForm.get('nombre').value,
      numeroIdentificacion: this.personaNaturalForm.get('numeroIdentificacion').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      telefonoProponente: this.personaNaturalForm.get('telefono').value,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,
      //tipoIdentificacionCodigo: 
    }

    this.procesoSeleccion.procesoSeleccionProponente.push( proponente );
    
    this.guardar.emit(null);
    //console.log(this.personaNaturalForm.value);
  }

  onSubmitPersonaJuridicaIndividual() {

    this.procesoSeleccion.procesoSeleccionProponente = [];
    let proponente: ProcesoSeleccionProponente = {

      procesoSeleccionProponenteId: this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,

      nombreProponente: this.personaJuridicaIndividualForm.get('nombre').value,
      numeroIdentificacion: this.personaJuridicaIndividualForm.get('numeroIdentificacion').value,
      nombreRepresentanteLegal: this.personaJuridicaIndividualForm.get('representanteLegal').value,
      cedulaRepresentanteLegal: this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').value,
      localizacionIdMunicipio: this.personaJuridicaIndividualForm.get('municipio').value ? this.personaJuridicaIndividualForm.get('municipio').value.localizacionId : null,
      direccionProponente: this.personaJuridicaIndividualForm.get('direccion').value,
      telefonoProponente: this.personaJuridicaIndividualForm.get('telefono').value,
      emailProponente: this.personaJuridicaIndividualForm.get('correoElectronico').value,
      
    }

    this.procesoSeleccion.procesoSeleccionProponente.push( proponente );
    
    this.guardar.emit(null);
    //console.log(this.personaNaturalForm.value);
  }

  validacionesUnionTemporal( porcentaje: number): string {
    let mensaje = '';

    if ( porcentaje != 100 )
      mensaje = 'Los porcentajes de participación no suman 100%.'

    return mensaje;
  }

  onSubmitUnionTemporal() {

    let porcentaje: number = 0;

    this.procesoSeleccion.procesoSeleccionProponente = [];
    this.procesoSeleccion.procesoSeleccionIntegrante = [];

    let listaIntegrantes =  this.unionTemporalForm.get('entidades') as FormArray;

    let proponente: ProcesoSeleccionProponente = {

      procesoSeleccionProponenteId: this.unionTemporalForm.get('procesoSeleccionProponenteId').value,
      procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
      tipoProponenteCodigo: this.tipoProponente.value ? this.tipoProponente.value.codigo : null,

      nombreProponente: this.unionTemporalForm.get('nombreConsorcio').value,
      numeroIdentificacion: this.unionTemporalForm.get('numeroIdentificacion').value,
      nombreRepresentanteLegal: this.unionTemporalForm.get('nombre').value,
      cedulaRepresentanteLegal: this.unionTemporalForm.get('cedulaRepresentanteLegal').value,
      localizacionIdMunicipio: this.unionTemporalForm.get('municipio').value ? this.unionTemporalForm.get('municipio').value.localizacionId : null,
      direccionProponente: this.unionTemporalForm.get('direccion').value,
      telefonoProponente: this.unionTemporalForm.get('telefono').value,
      emailProponente: this.unionTemporalForm.get('correoElectronico').value,
      
    }

    listaIntegrantes.controls.forEach( control => {
      let integrante: ProcesoSeleccionIntegrante = {
        nombreIntegrante: control.get('nombre').value,
        porcentajeParticipacion: control.get('porcentaje').value,
        procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
        procesoSeleccionIntegranteId: control.get('procesoSeleccionIntegranteId').value,

      }

      porcentaje = porcentaje + integrante.porcentajeParticipacion;

      this.procesoSeleccion.procesoSeleccionIntegrante.push( integrante );
    })

    let mensajeValidaciones = this.validacionesUnionTemporal( porcentaje );
    if (mensajeValidaciones.length > 0){
       this.openDialog('', mensajeValidaciones); 
       return false;
    }

    this.procesoSeleccion.procesoSeleccionProponente.push( proponente );
    
    this.guardar.emit(null);
  }

  cargarRegistro(){

    this.ngOnInit().then( () => 
        { 
          this.procesoSeleccion.procesoSeleccionProponente.forEach( proponente => {
            let tipoProponente = this.listaProponentes.find( p => p.codigo == proponente.tipoProponenteCodigo )
            if (tipoProponente) this.tipoProponente.setValue( tipoProponente );

            let idMunicipio = proponente.localizacionIdMunicipio ? proponente.localizacionIdMunicipio.toString() : "00000";
            let departamentoSeleccionado = this.listaDepartamentos.find( d => d.localizacionId == idMunicipio.substring(0,5) );
            

            this.commonService.listaMunicipiosByIdDepartamento( idMunicipio.substring(0,5) ).subscribe( listMun => {
              this.listaMunicipios = listMun;
              let municipio = listMun.find( m => m.localizacionId == proponente.localizacionIdMunicipio )

              switch (proponente.tipoProponenteCodigo)
            {
              case "1": {

                this.personaNaturalForm.get('municipio').setValue( municipio );
                this.personaNaturalForm.get('depaetamento').setValue( departamentoSeleccionado );
                this.personaNaturalForm.get('procesoSeleccionProponenteId').setValue( proponente.procesoSeleccionProponenteId );
                this.personaNaturalForm.get('direccion').setValue( proponente.direccionProponente );
                this.personaNaturalForm.get('correoElectronico').setValue( proponente.emailProponente );

                this.personaNaturalForm.get('municipio').setValue( proponente.localizacionIdMunicipio );

                this.personaNaturalForm.get('nombre').setValue( proponente.nombreProponente );
                this.personaNaturalForm.get('numeroIdentificacion').setValue( proponente.numeroIdentificacion );
                this.personaNaturalForm.get('telefono').setValue( proponente.telefonoProponente );
                
              }
              case "2": {
                this.personaJuridicaIndividualForm.get('depaetamento').setValue( departamentoSeleccionado );
                this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').setValue( proponente.procesoSeleccionProponenteId );
                this.personaJuridicaIndividualForm.get('nombre').setValue( proponente.nombreProponente );
                this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue( proponente.numeroIdentificacion );
                this.personaJuridicaIndividualForm.get('representanteLegal').setValue( proponente.nombreRepresentanteLegal );
                this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').setValue( proponente.cedulaRepresentanteLegal );
                this.personaJuridicaIndividualForm.get('municipio').setValue( municipio );                
                this.personaJuridicaIndividualForm.get('direccion').setValue( proponente.direccionProponente );
                this.personaJuridicaIndividualForm.get('telefono').setValue( proponente.telefonoProponente );
                this.personaJuridicaIndividualForm.get('correoElectronico').setValue( proponente.emailProponente );
              }
              case "4": {

                let listaIntegrantes =  this.unionTemporalForm.get('entidades') as FormArray;
                
                this.unionTemporalForm.get('depaetamento').setValue( departamentoSeleccionado );
                this.unionTemporalForm.get('procesoSeleccionProponenteId').setValue( proponente.procesoSeleccionProponenteId ),
                this.unionTemporalForm.get('nombreConsorcio').setValue( proponente.nombreProponente );
                this.unionTemporalForm.get('numeroIdentificacion').setValue( proponente.numeroIdentificacion );
                this.unionTemporalForm.get('nombre').setValue( proponente.nombreRepresentanteLegal );
                this.unionTemporalForm.get('cedulaRepresentanteLegal').setValue( proponente.cedulaRepresentanteLegal );
                this.unionTemporalForm.get('municipio').setValue( municipio );
                this.unionTemporalForm.get('direccion').setValue( proponente.direccionProponente );
                this.unionTemporalForm.get('telefono').setValue( proponente.telefonoProponente );
                this.unionTemporalForm.get('correoElectronico').setValue( proponente.emailProponente );

                this.procesoSeleccion.procesoSeleccionIntegrante.forEach( integrante => {
                  let control = this.createIntegrante();
                  control.get('nombre').setValue( integrante.nombreIntegrante );
                  control.get('porcentaje').setValue( integrante.porcentajeParticipacion );
                  control.get('procesoSeleccionIntegranteId').setValue( integrante.procesoSeleccionIntegranteId );

                  listaIntegrantes.push( control );  

                })

                console.log( this.procesoSeleccion );
                this.unionTemporalForm.get('cuantasEntidades').setValue( listaIntegrantes.length ); 
              }
            }
            })
          })
        });
  }

  setValueAutocomplete(proponente:any)
  {
    console.log(proponente.tipoProponenteCodigo);
    let idMunicipio = proponente.localizacionIdMunicipio ? proponente.localizacionIdMunicipio.toString() : "00000";
    let departamentoSeleccionado = this.listaDepartamentos.find( d => d.localizacionId == idMunicipio.substring(0,5) );
    this.commonService.listaMunicipiosByIdDepartamento( departamentoSeleccionado.localizacionId ).subscribe( listMun => {
      this.listaMunicipios = listMun;
      let municipio = this.listaMunicipios.find( m => m.localizacionId == proponente.localizacionIdMunicipio )
      switch (proponente.tipoProponenteCodigo)
      {
        case "1": {
          
          this.personaNaturalForm.get('municipio').setValue( municipio );
          this.personaNaturalForm.get('depaetamento').setValue( departamentoSeleccionado );
          this.personaNaturalForm.get('procesoSeleccionProponenteId').setValue( proponente.procesoSeleccionProponenteId );
          this.personaNaturalForm.get('direccion').setValue( proponente.direccionProponente );
          this.personaNaturalForm.get('correoElectronico').setValue( proponente.emailProponente );
  
          //this.personaNaturalForm.get('municipio').setValue( proponente.localizacionIdMunicipio );
  
          this.personaNaturalForm.get('nombre').setValue( proponente.nombreProponente );
          this.personaNaturalForm.get('numeroIdentificacion').setValue( proponente.numeroIdentificacion );
          this.personaNaturalForm.get('telefono').setValue( proponente.telefonoProponente );                    
          
        }
        case "2": {
          this.personaJuridicaIndividualForm.get('depaetamento').setValue( departamentoSeleccionado );
          this.personaJuridicaIndividualForm.get('municipio').setValue( municipio );                
          this.personaJuridicaIndividualForm.get('procesoSeleccionProponenteId').setValue( proponente.procesoSeleccionProponenteId );
          this.personaJuridicaIndividualForm.get('nombre').setValue( proponente.nombreProponente );
          this.personaJuridicaIndividualForm.get('numeroIdentificacion').setValue( proponente.numeroIdentificacion );
          this.personaJuridicaIndividualForm.get('representanteLegal').setValue( proponente.nombreRepresentanteLegal );
          this.personaJuridicaIndividualForm.get('cedulaRepresentanteLegal').setValue( proponente.cedulaRepresentanteLegal );
          
          this.personaJuridicaIndividualForm.get('direccion').setValue( proponente.direccionProponente );
          this.personaJuridicaIndividualForm.get('telefono').setValue( proponente.telefonoProponente );
          this.personaJuridicaIndividualForm.get('correoElectronico').setValue( proponente.emailProponente );
        }
        case "4": {
  
          let listaIntegrantes =  this.unionTemporalForm.get('entidades') as FormArray;
          
          this.unionTemporalForm.get('depaetamento').setValue( departamentoSeleccionado );
          this.unionTemporalForm.get('procesoSeleccionProponenteId').setValue( proponente.procesoSeleccionProponenteId ),
          this.unionTemporalForm.get('nombreConsorcio').setValue( proponente.nombreProponente );
          this.unionTemporalForm.get('numeroIdentificacion').setValue( proponente.numeroIdentificacion );
          this.unionTemporalForm.get('nombre').setValue( proponente.nombreRepresentanteLegal );
          this.unionTemporalForm.get('cedulaRepresentanteLegal').setValue( proponente.cedulaRepresentanteLegal );
          this.unionTemporalForm.get('municipio').setValue( municipio );
          this.unionTemporalForm.get('direccion').setValue( proponente.direccionProponente );
          this.unionTemporalForm.get('telefono').setValue( proponente.telefonoProponente );
          this.unionTemporalForm.get('correoElectronico').setValue( proponente.emailProponente );
  
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
}
