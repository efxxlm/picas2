import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { debounceTime, map, startWith } from 'rxjs/operators';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { Dominio, CommonService, Localizacion } from 'src/app/core/_services/common/common.service';
import { Aportante, Proyecto } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DisponibilidadPresupuestal, DisponibilidadPresupuestalProyecto, ListAportantes } from 'src/app/_interfaces/budgetAvailability';

@Component({
  selector: 'app-nueva-solicitud-especial',
  templateUrl: './nueva-solicitud-especial.component.html',
  styleUrls: ['./nueva-solicitud-especial.component.scss']
})

export class NuevaSolicitudEspecialComponent implements OnInit {

  tipoSolicitudArray: Dominio[] = [];
  listaDepartamento: any[] = [];
  listaMunicipio: any[] = [];
  listaAportante: ListAportantes[] = [];
  disponibilidadPresupuestal: DisponibilidadPresupuestal;
  proyectoEncontrado: boolean = false;
  seRealizoPeticion: boolean = false;
  seRecibioAportante: boolean = false;
  tipoSeleccionado: Dominio;
  proyecto: Proyecto;
  contrato: any;
  tipoAportante = {
    aportanteFfie: 6,
    aportanteEt: 9,
    aportanteTercero: 10
  };
  tipoSolicitudCodigos = {
    solicitudExpensas: '1',
    solicitudOtrosCostos: '2'
  };
  tipoAportantes: any[] = [];
  nombreAportantes: any[] = [];
  myFilter = new FormControl();
  listaContrato: any[] = [];
  filteredContrato: Observable<string[]>;

  addressForm = this.fb.group({
    disponibilidadPresupuestalId: [  ],
    disponibilidadPresupuestalProyectoId: [  ],
    tipo: [ null, Validators.required ],
    objeto: [ null, Validators.required ],
    numeroRadicado: [ null, Validators.compose( [ Validators.minLength(1), Validators.maxLength(15) ] ) ],
    cartaAutorizacionET: ['', Validators.required],
    numeroContrato: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(10) ] ) ],
    departemento: [ null, Validators.required ],
    municipio: [ null, Validators.required ],
    llaveMEN: [ null, Validators.required ],
    tipoAportante: [ null, Validators.required ],
    observacionLimiteEspecial: [ null ],
    nombreAportante: [ null, Validators.required ],
    valor: [ 0, Validators.compose( [ Validators.minLength(4), Validators.maxLength(20) ] ) ],
    url: [ null, Validators.required ]
  });

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

  configLimiteEspecial = {
    toolbar: []
  };
  disponibilidadaeditar: DisponibilidadPresupuestal;
  esEdicion: boolean=false;

  constructor(  private fb: FormBuilder,
                private commonService: CommonService,
                private budgetAvailabilityService: BudgetAvailabilityService,
                public dialog: MatDialog,
                private router: Router, 
                private activatedRoute: ActivatedRoute ) 
  {
    this.getValueChanges();
    forkJoin([
      this.commonService.listaTipoDDPEspecial(),
      this.commonService.listaDepartamentos(),  
      this.commonService.listaTipoAportante()    
    ])
      .subscribe(respuesta => {
        this.tipoSolicitudArray = respuesta[0];
        this.listaDepartamento = respuesta[1];
        this.tipoAportantes = respuesta[2];
        this.budgetAvailabilityService.getContratosList().subscribe(
          result=>{this.listaContrato=result;}
        );
        
        this.filteredContrato = this.myFilter.valueChanges.pipe(
          startWith(''),
          map(value => this._filter(value))
        );
        if ( this.activatedRoute.snapshot.params.id !== '0' ) {
          this.getRegistro( this.activatedRoute.snapshot.params.id );
        };
      });
  };

  

  ngOnInit(): void {    
  };

  getValueChanges () {
    this.addressForm.get( 'llaveMEN' ).valueChanges
      .pipe(
        debounceTime( 2000 )
      )
      .subscribe(
        () => this.buscarProyecto()
      );
/**deprecated es un autocompletar*/
/*
    this.addressForm.get( 'numeroContrato' ).valueChanges
      .pipe(
        debounceTime( 2000 )
      )
      .subscribe( response => {
      
        if ( response.length >= 3 ) {
          this.budgetAvailabilityService.getNumeroContrato( response )
            .subscribe(
              ( response: any[] ) => {
                this.contrato = response;
                for ( let contratacionProyecto of this.contrato.contratacion.contratacionProyecto ) {
                  if ( contratacionProyecto.proyecto.proyectoAportante[0].aportante.tipoAportanteId === this.tipoAportante.aportanteFfie ) {
                    this.tipoAportantes.push( { value: this.tipoAportante.aportanteFfie, nombre: 'FFIE' } );
                  };
                  if ( contratacionProyecto.proyecto.proyectoAportante[0].aportante.tipoAportanteId === this.tipoAportante.aportanteEt ) {
                    //por integrar
                  };
                  if ( contratacionProyecto.proyecto.proyectoAportante[0].aportante.tipoAportanteId === this.tipoAportante.aportanteTercero ) {
                    //por integrar
                  }
                }
                console.log( this.contrato );
              },
              err => this.openDialog( '', '<b>Este número de contrato no existe por favor verifique los datos registrados.</b>' )
            );
        }

      } );
*/
      this.addressForm.get( 'tipoAportante' ).valueChanges
        .subscribe( value => {
          this.nombreAportantes = [];      
          console.log(this.contrato);    
          this.contrato.listAportantes.forEach( contratacion => {
            let tipoapo=this.tipoAportantes.filter(x=>x.codigo==value);
            if(tipoapo[0].nombre==contratacion.tipoAportante)
            {
              this.nombreAportantes.push( { value:contratacion.cofinanciacionAportanteId, 
                nombre: contratacion.nombre,
                aportanteId: contratacion.cofinanciacionAportanteId } );
            }            
          });
          if(this.disponibilidadaeditar)
          {
            console.log("############33");
            console.log(this.nombreAportantes);
            let nombreAportante= this.nombreAportantes.filter(x=>x.aportanteId==this.disponibilidadaeditar.aportanteId);                                   
            console.log(nombreAportante[0]);
            this.addressForm.get( 'nombreAportante' ).setValue(nombreAportante[0]);     
          }
        } );
  };

  getRegistro ( id: number ) {
    this.budgetAvailabilityService.getDetailInfoAdditionalById( id )
      .subscribe( disponibilidad => {
        this.tipoSeleccionado = this.tipoSolicitudArray.find( t => t.codigo == disponibilidad.tipoSolicitudEspecialCodigo );
      this.disponibilidadaeditar=disponibilidad;
        console.log( disponibilidad );

        if ( disponibilidad.tipoSolicitudEspecialCodigo === this.tipoSolicitudCodigos.solicitudExpensas ) {
          this.addressForm.get( 'disponibilidadPresupuestalId' ).setValue(disponibilidad.disponibilidadPresupuestalId);
          this.addressForm.get( 'tipo' ).setValue( this.tipoSeleccionado );
          this.addressForm.get( 'objeto' ).setValue( disponibilidad.objeto );
          this.addressForm.get( 'numeroRadicado' ).setValue( disponibilidad.numeroRadicadoSolicitud );
          this.addressForm.get( 'cartaAutorizacionET' ).setValue( disponibilidad.cuentaCartaAutorizacion );
  
          if (disponibilidad.disponibilidadPresupuestalProyecto.length > 0){
            console.log(disponibilidad.disponibilidadPresupuestalProyecto[0].proyecto[ 'departamentoObj' ].localizacionId);
            let depto=this.listaDepartamento.filter(x=>x.localizacionId==disponibilidad.disponibilidadPresupuestalProyecto[0].proyecto[ 'departamentoObj' ].localizacionId);
            if (depto[0]) {
              this.commonService.listaMunicipiosByIdDepartamento(depto[0].localizacionId)
                .subscribe(listaMunicipios => {
                  this.listaMunicipio = listaMunicipios;
                  let muni=this.listaMunicipio.filter(x=>x.localizacionId==disponibilidad.disponibilidadPresupuestalProyecto[0].proyecto[ 'municipioObj' ].localizacionId);
                  this.addressForm.get( 'municipio' ).setValue( muni[0]);
                })
            };
            
            this.addressForm.get( 'departemento' ).setValue( depto[0] );
            
            this.addressForm.get( 'disponibilidadPresupuestalProyectoId' ).setValue(disponibilidad.disponibilidadPresupuestalProyecto[0].disponibilidadPresupuestalProyectoId);
            this.addressForm.get( 'llaveMEN' ).setValue(disponibilidad.disponibilidadPresupuestalProyecto[0].proyecto.llaveMen);
            //
            
            let nombreAportante= this.nombreAportantes.filter(x=>x.aportanteId==disponibilidad.aportanteId);
            this.addressForm.get( 'nombreAportante' ).setValue(nombreAportante[0]);
            this.addressForm.get( 'valor' ).setValue( disponibilidad.valorAportante ? disponibilidad.valorAportante : 0 );
            this.addressForm.get( 'url' ).setValue( disponibilidad.urlSoporte ? disponibilidad.urlSoporte : null );
          }
        };
        if ( disponibilidad.tipoSolicitudEspecialCodigo === this.tipoSolicitudCodigos.solicitudOtrosCostos ) {
          this.addressForm.get( 'disponibilidadPresupuestalId' ).setValue( disponibilidad.disponibilidadPresupuestalId );
          this.addressForm.get( 'tipo' ).setValue( this.tipoSeleccionado );
          this.addressForm.get( 'objeto' ).setValue( disponibilidad.objeto );
          this.addressForm.get('numeroRadicado').setValue( disponibilidad.numeroRadicadoSolicitud );
          this.addressForm.get( 'numeroContrato' ).setValue( disponibilidad.numeroContrato );
          this.myFilter.setValue( disponibilidad.numeroContrato );
          this.addressForm.get( 'observacionLimiteEspecial' ).setValue( disponibilidad.limitacionEspecial ? disponibilidad.limitacionEspecial : null );
         
          let tipoaportante=this.tipoAportantes.filter(x=>x.dominioId==disponibilidad.aportante.tipoAportanteId);
          this.addressForm.get( 'valor' ).setValue( disponibilidad.valorAportante ? disponibilidad.valorAportante : 0 );
          this.addressForm.get( 'url' ).setValue( disponibilidad.urlSoporte ? disponibilidad.urlSoporte : null );
          this.budgetAvailabilityService.getContratoByNumeroContrato( disponibilidad.numeroContrato )
            .subscribe(
              ( response: any[] ) => {
                this.contrato = response;          
                this.contrato.listAportantes.forEach( contratacion => {
                  this.nombreAportantes.push( { value:contratacion.cofinanciacionAportanteId, nombre: contratacion.nombre, aportanteId: contratacion.cofinanciacionAportanteId } );                  
                } );
                this.addressForm.get( 'tipoAportante' ).setValue(tipoaportante[0].codigo);
                console.log("############33");
                console.log(this.nombreAportantes);
                let nombreAportante= this.nombreAportantes.filter(x=>x.aportanteId==disponibilidad.aportanteId);                                   
                console.log(nombreAportante[0]);
                this.addressForm.get( 'nombreAportante' ).setValue(nombreAportante[0]);            
              },
              //err => this.openDialog( '', '<b>Este número de contrato no existe por favor verifique los datos registrados.</b>' )
            );          
        };
      } );
  };

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  buscarProyecto( ) {

    this.proyectoEncontrado = false;
    this.proyecto = {};
    this.addressForm.get('valor').setValue('')
    this.addressForm.get('nombreAportante').setValue('');

    let llameMen: string = this.addressForm.get('llaveMEN').value;

    if (llameMen) {
      if ( llameMen.length >= 3 ) {
        this.budgetAvailabilityService.searchLlaveMEN(llameMen)
        .subscribe(
          listaProyectos => {
            if ( listaProyectos.length === 0 ) {
              this.openDialog('', '<b>Esta llave no existe por favor verifique los datos registrados</b>');
              return;
            };
            if ( listaProyectos.length > 0 ) {
              this.proyectoEncontrado = true;
              this.proyecto = listaProyectos[0];
              this.budgetAvailabilityService.getAportanteTerritorial( this.proyecto.proyectoId, this.tipoAportante.aportanteEt )
                .subscribe(
                  ( aportante: any ) => {
                    if ( aportante.length === 0 ) {
                      this.openDialog( '', '<b>El proyecto no tiene una entidad territorial como aportante.<br><br>La solicitud no se puede completar.</b>' );
                      this.seRecibioAportante = false;
                      this.addressForm.get('llaveMEN').setValue("");
                      return;
                    };
                    
                    if ( aportante[0].departamento !== undefined && aportante[0].municipio !== undefined ) {
                      this.nombreAportantes.push(
                        { 
                          value: aportante[0].tipoAportanteId, 
                          nombre: `Alcaldía de ${ aportante[0].municipio.descripcion }`,
                          aportanteId: aportante[0].cofinanciacionAportanteId
                        }
                      );
                    };
                    if ( aportante[0].departamento !== undefined && aportante[0].municipio === undefined ) {
                      this.nombreAportantes.push( 
                        { 
                          value: aportante[0].tipoAportanteId, 
                          nombre: `Gobernación de ${ aportante[0].departamento.descripcion }`,
                          aportanteId: aportante[0].cofinanciacionAportanteId
                        }
                      );
                    };

                    this.seRecibioAportante = true;
                    if ( this.activatedRoute.snapshot.params.id !== '0' ) {
                      let nombreAportante= this.nombreAportantes.filter(x=>x.aportanteId==this.disponibilidadaeditar.aportanteId);
                      this.addressForm.get( 'nombreAportante' ).setValue(nombreAportante[0]);
                      this.addressForm.get( 'valor' ).setValue( this.disponibilidadaeditar.valorAportante ? this.disponibilidadaeditar.valorAportante : 0 );
                      this.addressForm.get( 'url' ).setValue( this.disponibilidadaeditar.urlSoporte ? this.disponibilidadaeditar.urlSoporte : null );
                    }
                  },
                  err => this.openDialog( '', `<b>${err.message}</b>` )
                );
            };
          }, 
          err => this.openDialog('', `<b>${err.message}</b>`)
        )
      }
    }
  };

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  };

  changeDepartamento() {
    let departamento = this.addressForm.get('departemento').value;
    if (departamento) {
      this.commonService.listaMunicipiosByIdDepartamento(departamento.localizacionId)
        .subscribe(listaMunicipios => {
          this.listaMunicipio = listaMunicipios;
        })
    };
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  };

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  onSubmit() {
    //if (this.addressForm.valid) {

      let tipoDDP: Dominio = this.addressForm.get('tipo').value;


      if (tipoDDP) {

        switch (tipoDDP.codigo) {
          case "1": //expensas

            let disponibilidad: DisponibilidadPresupuestal = {
              disponibilidadPresupuestalId: this.addressForm.get('disponibilidadPresupuestalId').value,
              tipoSolicitudCodigo: "2", //especial
              tipoSolicitudEspecialCodigo: tipoDDP.codigo,
              objeto: this.addressForm.get('objeto').value,
              numeroRadicadoSolicitud: this.addressForm.get('numeroRadicado').value,
              aportanteId: this.addressForm.get('nombreAportante').value ? this.addressForm.get('nombreAportante').value.aportanteId : null,              
              valorAportante: this.addressForm.get('valor').value,
              cuentaCartaAutorizacion: this.addressForm.get('cartaAutorizacionET').value,
              urlSoporte: this.addressForm.get('url').value,
              disponibilidadPresupuestalProyecto:[{proyectoId:this.proyecto.proyectoId,
                disponibilidadPresupuestalProyectoId:this.disponibilidadaeditar?this.disponibilidadaeditar.disponibilidadPresupuestalProyecto[0].disponibilidadPresupuestalProyectoId:null}]
            };
            console.log( disponibilidad );
            this.budgetAvailabilityService.createUpdateDisponibilidaPresupuestalEspecial( disponibilidad )
              .subscribe( 
                respuesta => {
                  this.openDialog( '', respuesta.message )
                  if ( respuesta.code == "200" )
                    this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudEspecial'])
                },
                err => this.openDialog( '', err.message )
              );

            break;
          case "2":

            let disponibilidadPresupuestal: DisponibilidadPresupuestal = {
              disponibilidadPresupuestalId: this.addressForm.get('disponibilidadPresupuestalId').value,
              tipoSolicitudCodigo: "2", //especial
              tipoSolicitudEspecialCodigo: tipoDDP.codigo,
              objeto: this.addressForm.get('objeto').value,
              contratacionId:this.contrato.contratacionId,
              numeroContrato: this.contrato.numeroContrato,
              numeroRadicadoSolicitud: this.addressForm.get('numeroRadicado').value,
              aportanteId: this.addressForm.get('nombreAportante').value ? this.addressForm.get('nombreAportante').value.aportanteId : null,
              valorAportante: this.addressForm.get('valor').value ? this.addressForm.get('valor').value : null,
              urlSoporte: this.addressForm.get('url').value ? this.addressForm.get('url').value : null,
              limitacionEspecial: this.addressForm.get( 'observacionLimiteEspecial' ).value ? this.addressForm.get( 'observacionLimiteEspecial' ).value : null
            };
            console.log( disponibilidadPresupuestal );
            this.budgetAvailabilityService.createUpdateDisponibilidaPresupuestalEspecial( disponibilidadPresupuestal )
              .subscribe(
                response => {
                  this.openDialog( '', `<b>${response.message}</b>` );
                  this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudEspecial']);
                },
                err => this.openDialog( '', `<b>${err.message}</b>` )
              )

           break;  
        }

      };
    //}
  }
  
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();    
    if(value!="")
    {      
      let filtroportipo:string[]=[];
      this.listaContrato.forEach(element => {        
        if(!filtroportipo.includes(element.numeroContrato))
        {
          filtroportipo.push(element.numeroContrato);
        }
      });
      let ret= filtroportipo.filter(x=> x.toLowerCase().indexOf(filterValue) === 0);      
      return ret;
    }
    else
    {
      return [];
    }
    
  }
  seleccionAutocomplete(nombre: string) {
    let lista: any[] = [];
    this.listaContrato.forEach(element => {
      if (element.numeroContrato) {
        lista.push(element);
      }
    });

    let ret = lista.filter(x => x.numeroContrato.toLowerCase() === nombre.toLowerCase());
    console.log(ret);
    //reuso
    this.budgetAvailabilityService.getContratoByNumeroContrato( nombre )
            .subscribe(
              ( response: any[] ) => {
                this.contrato = response;
                if(this.contrato.listAportantes.length)
                {
                  this.nombreAportantes=this.contrato.listaAportante;
                  let aportantes=this.tipoAportantes;
                  this.tipoAportantes=[];
                  this.contrato.listAportantes.forEach(element => {
                    let aportante=aportantes.filter(x=>x.nombre==element.tipoAportante);
                    this.tipoAportantes.push(aportante[0]);
                  });
                  console.log(this.tipoAportantes);
                  console.log(aportantes);
                }
                console.log( this.contrato );
              },
              err => this.openDialog( '', '<b>Este número de contrato no existe por favor verifique los datos registrados.</b>' )
            );
    //this.addressForm.get("numeroContrato").setValue();
    this.contrato=ret[0];
  }

};