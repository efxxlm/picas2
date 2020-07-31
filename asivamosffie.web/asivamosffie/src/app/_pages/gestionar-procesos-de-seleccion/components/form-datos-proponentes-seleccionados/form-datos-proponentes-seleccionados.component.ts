import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormControl, FormArray, FormGroup } from '@angular/forms';
import { ProcesoSeleccion, ProcesoSeleccionProponente, ProcesoSeleccionIntegrante } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { Dominio, Localizacion, CommonService } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';

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

  get entidades() {
    return this.unionTemporalForm.get('entidades') as FormArray;
  }


  constructor(
              private fb: FormBuilder,
              private commonService: CommonService,

             ) 
  {
    this.declararSelect();
  }
  ngOnInit(): void {
    
    forkJoin([
      
      this.commonService.listaTipoProponente(),
      this.commonService.listaDepartamentos(),

    ]).subscribe( respuesta => {
      this.listaProponentes = respuesta[0];
      this.listaDepartamentos = respuesta[1];
    })

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

  onSubmitUnionTemporal() {
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
      this.procesoSeleccion.procesoSeleccionIntegrante.push( integrante );
    })


    this.procesoSeleccion.procesoSeleccionProponente.push( proponente );
    
    this.guardar.emit(null);
  }

  cargarRegistro(){

    setTimeout( () => 
        { 
          let tipoProponente = this.listaProponentes.find( p => p.codigo == this.procesoSeleccion.tipoProcesoCodigo )
          if (tipoProponente) this.tipoProponente.setValue( tipoProponente );

          this.procesoSeleccion.procesoSeleccionProponente.forEach( proponente => {
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

                this.unionTemporalForm.get('cuantasEntidades').setValue( listaIntegrantes.length ); 
              }
            }

              

            })

            
          })

            // let listaCotizaciones = this.addressForm.get('cotizaciones') as FormArray

            // listaCotizaciones.clear();
            // this.addressForm.get('cuantasCotizaciones').setValue( this.procesoSeleccion.cantidadCotizaciones )

            // this.procesoSeleccion.procesoSeleccionCotizacion.forEach( cotizacion => {
            //   let control = this.createCotizacion();

            //   control.get('descripcion').setValue( cotizacion.descripcion ),
            //   control.get('nombreOrganizacion').setValue( cotizacion.nombreOrganizacion ),
            //   control.get('procesoSeleccionCotizacionId').setValue( cotizacion.procesoSeleccionCotizacionId ),
            //   control.get('url').setValue( cotizacion.urlSoporte ),
            //   control.get('valor').setValue( cotizacion.valorCotizacion ),

            //   listaCotizaciones.push( control );
            // })

            // console.log('entro')
          
        }, 1000 );
  }
}
