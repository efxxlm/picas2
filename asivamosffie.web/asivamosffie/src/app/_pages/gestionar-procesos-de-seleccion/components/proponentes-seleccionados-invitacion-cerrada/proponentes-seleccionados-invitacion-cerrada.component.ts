import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ProcesoSeleccionService, ProcesoSeleccionProponente, ProcesoSeleccion, ProcesoSeleccionIntegrante } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';

@Component({
  selector: 'app-form-datos-proponentes-seleccionados-invitacion-cerrada',
  templateUrl: './proponentes-seleccionados-invitacion-cerrada.component.html',
  styleUrls: ['./proponentes-seleccionados-invitacion-cerrada.component.scss']
})
export class FormDatosProponentesSeleccionadosInvitacionCerradaComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 

  nombresProponentesList: ProcesoSeleccionProponente[] = [];
  idProponenteExistente: string;
  
  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2)])
    ],
    nombresProponentes: [null, Validators.required],
    tipoProponente: [null, Validators.required],
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    numeroIdentificacon: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    representanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    cedulaRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    departamentoRepresentanteLegal: [null, Validators.required],
    municipioRepresentanteLegal: [null, Validators.required],
    direccionPrincipalRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    telefonoRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    correoRepresentanteLegal: [null, [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(50),
      Validators.email,
      Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
    ]],
  });

  constructor(
              private fb: FormBuilder,
              private procesoSeleccionService: ProcesoSeleccionService
             ) 
  {

  }
  ngOnInit(){
    return new Promise( resolve => {
      this.procesoSeleccionService.getProcesoSeleccionProponentes().subscribe( lista => {

        if (!this.procesoSeleccion.procesoSeleccionProponente)
          this.procesoSeleccion.procesoSeleccionProponente = [];
          
        for ( let i = 0; i < this.procesoSeleccion.procesoSeleccionProponente.length; i++ ){ 
            lista = lista.filter( p => p.numeroIdentificacion != this.procesoSeleccion.procesoSeleccionProponente[i].numeroIdentificacion )
        }
        
        for ( let i = 0; i < this.procesoSeleccion.procesoSeleccionProponente.length; i++ ){ 
          lista = lista.filter( p => p.numeroIdentificacion != this.procesoSeleccion.procesoSeleccionProponente[i].numeroIdentificacion )
        }

        this.nombresProponentesList = lista;
        resolve();
      })
    })
  }

  changeProponente(){
    console.log(this.addressForm.get('nombresProponentes').value);
    
    if (!this.procesoSeleccion.procesoSeleccionProponente)
      this.procesoSeleccion.procesoSeleccionProponente = [];
    
    for( let i = 0; i < this.procesoSeleccion.procesoSeleccionProponente.length; i++)
    {
      if ( this.procesoSeleccion.procesoSeleccionProponente[i].procesoSeleccionProponenteId == "0" )
        this.procesoSeleccion.procesoSeleccionProponente.splice( i, 1);    
        
    }

    if ( this.addressForm.get('nombresProponentes').value != 'Nuevo' ){
      let elemento: ProcesoSeleccionProponente = this.addressForm.get('nombresProponentes').value;
      elemento.procesoSeleccionProponenteId = "0";
      elemento.procesoSeleccionId = this.procesoSeleccion.procesoSeleccionId;
      this.procesoSeleccion.procesoSeleccionProponente.push( elemento );    
      this.idProponenteExistente = this.addressForm.get('nombresProponentes').value.procesoSeleccionProponenteId; 
    }
  }

  cargarRegistro(){
    

    this.ngOnInit().then(() =>       
        { 
          this.addressForm.get('cuantosProponentes').setValue( this.procesoSeleccion.cantidadProponentesInvitados );      
        });
  }

  onSubmit() {
    
    this.addressForm.get('nombresProponentes').setValue( null );
    this.procesoSeleccion.cantidadProponentesInvitados = this.addressForm.get('cuantosProponentes').value;
    this.guardar.emit(null);
  }

  onSubmitNuevoProponente(){
    this.addressForm.get('nombresProponentes').setValue( null );

    this.guardar.emit(null);
  }

}
