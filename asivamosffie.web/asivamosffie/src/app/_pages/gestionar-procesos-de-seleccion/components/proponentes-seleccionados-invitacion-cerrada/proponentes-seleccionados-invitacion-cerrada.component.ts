import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ProcesoSeleccionService, ProcesoSeleccionProponente, ProcesoSeleccion, ProcesoSeleccionIntegrante } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { Localizacion, CommonService } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-form-datos-proponentes-seleccionados-invitacion-cerrada',
  templateUrl: './proponentes-seleccionados-invitacion-cerrada.component.html',
  styleUrls: ['./proponentes-seleccionados-invitacion-cerrada.component.scss']
})
export class FormDatosProponentesSeleccionadosInvitacionCerradaComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Input() editar:boolean;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 

  nombresProponentesList: ProcesoSeleccionProponente[] = [];
  listaDepartamentos: Localizacion[] = [];
  idProponenteExistente: string;
  
  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2)])
    ],
    nombresProponentes: [null, Validators.required],
    tipoProponente: [null, Validators.required],
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(100)])
    ],
    numeroIdentificacon: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    representanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(100)])
    ],
    cedulaRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    departamentoRepresentanteLegal: [null, Validators.required],
    municipioRepresentanteLegal: [null, Validators.required],
    direccionPrincipalRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(100)])
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
  nuevo: boolean=false;

  constructor(
              private fb: FormBuilder,
              private procesoSeleccionService: ProcesoSeleccionService,
              private commonService: CommonService,
              private dialog: MatDialog, 
             ) 
  {

  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  validaMinimo3()
  {
    if(this.addressForm.get("cuantosProponentes").value!="" && this.addressForm.get("cuantosProponentes").value!=null &&
    this.addressForm.get("cuantosProponentes").value!= undefined && this.addressForm.get("cuantosProponentes").value<3)
    {
      this.openDialog("","<b>La cantidad de proponentes debe ser mayor o igual al 3</b>");
    }
  }
  ngOnInit(){
    return new Promise( resolve => {

      forkJoin([
        this.procesoSeleccionService.getProcesoSeleccionProponentes(),
        this.commonService.listaDepartamentos(),

      ]).subscribe( resultado => {

        let lista = resultado[0];
        this.listaDepartamentos = resultado[1]

        if (!this.procesoSeleccion.procesoSeleccionProponente)
          this.procesoSeleccion.procesoSeleccionProponente = [];
          
        for ( let i = 0; i < this.procesoSeleccion.procesoSeleccionProponente.length; i++ ){ 
            lista = lista.filter( p => p.numeroIdentificacion != this.procesoSeleccion.procesoSeleccionProponente[i].numeroIdentificacion )
        }
        
        for ( let i = 0; i < this.procesoSeleccion.procesoSeleccionProponente.length; i++ ){ 
          lista = lista.filter( p => p.numeroIdentificacion != this.procesoSeleccion.procesoSeleccionProponente[i].numeroIdentificacion )
        }

        this.nombresProponentesList = lista;

        this.procesoSeleccion.procesoSeleccionProponente.forEach( p => {
          if (p.localizacionIdMunicipio)
          this.commonService.listaMunicipiosByIdDepartamento( p.localizacionIdMunicipio.substring(0,5) )
            .subscribe( municipios =>{
               let nombreMunicipio = municipios.find( m => m.localizacionId == p.localizacionIdMunicipio )
               let nombreDepartamento = this.listaDepartamentos.find( d => d.localizacionId == p.localizacionIdMunicipio.substring(0,5) )

               p.nombreMunicipio = nombreMunicipio.descripcion;
               p.nombreDepartamento = nombreDepartamento.descripcion; 
            })
          
        });

        console.log(this.nombresProponentesList)
        resolve();
      })
    })
  }

  changeProponente($event:any){
    
    console.log(this.addressForm.get('cuantosProponentes').value);
    //this.procesoSeleccion.procesoSeleccionProponente=[];
    console.log(this.procesoSeleccion.procesoSeleccionProponente.length);
    console.log(this.addressForm.get('nombresProponentes').value.length);
    console.log(this.addressForm.get('nombresProponentes').value);
    if(this.addressForm.get('cuantosProponentes').value>0)
    {
      if(this.procesoSeleccion.procesoSeleccionProponente.length>this.addressForm.get('nombresProponentes').value.length)
      {
        this.procesoSeleccion.procesoSeleccionProponente=[];
      }
      if(this.addressForm.get('cuantosProponentes').value>=this.addressForm.get('nombresProponentes').value.length
      && this.addressForm.get('cuantosProponentes').value>this.procesoSeleccion.procesoSeleccionProponente.length)
      {
        this.addressForm.get('nombresProponentes').value.forEach(element => {   
          console.log(element);     
          if ( element != 'Nuevo' ){
            let elemento: ProcesoSeleccionProponente = element;
            if(elemento.procesoSeleccionProponenteId!="0" && this.procesoSeleccion.procesoSeleccionId!=elemento.procesoSeleccionId)
            {
              elemento.procesoSeleccionProponenteId = "0";
              elemento.procesoSeleccionId = this.procesoSeleccion.procesoSeleccionId;              
            }
            if(!this.procesoSeleccion.procesoSeleccionProponente.includes(elemento))
            {
              this.procesoSeleccion.procesoSeleccionProponente.push( elemento );    
            }
            
            this.idProponenteExistente = element.procesoSeleccionProponenteId; 
            console.log(this.procesoSeleccion.procesoSeleccionProponente);
          }
          else{
            this.nuevo=true;
          }
        });
      }
      else
      {
        this.openDialog("","<b>Ya completo los "+this.addressForm.get('cuantosProponentes').value+" proponentes indicados.</b>");        
        this.addressForm.get('nombresProponentes').setValue(this.procesoSeleccion.procesoSeleccionProponente);
        return;
      }
    }
    
    
    
    /*for( let i = 0; i < this.procesoSeleccion.procesoSeleccionProponente.length; i++)
    {
      if ( this.procesoSeleccion.procesoSeleccionProponente[i].procesoSeleccionProponenteId == "0" )
        this.procesoSeleccion.procesoSeleccionProponente.splice( i, 1);    
        
    }*/

    
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  cargarRegistro(){
    

    this.ngOnInit().then(() =>       
        { 
          this.addressForm.get('cuantosProponentes').setValue( this.procesoSeleccion.cantidadProponentesInvitados );      
          let proceso:ProcesoSeleccionProponente[]=[];
          
          this.procesoSeleccion.procesoSeleccionProponente.forEach(element => {
            //busco 
            console.log("busco "+element.procesoSeleccionProponenteId+" en");
            console.log(this.nombresProponentesList);
            let nombre = this.nombresProponentesList.filter(x=>x.procesoSeleccionProponenteId==element.procesoSeleccionProponenteId);
            if(nombre.length>0)
            {
              proceso.push(nombre[0]);
            }     
            else
            {
              this.nombresProponentesList.push(element);
              proceso.push(element);
            }       
          });
          console.log(proceso)
          this.addressForm.get('nombresProponentes').setValue(proceso);
          console.log(this.addressForm.get('nombresProponentes').value);
        });
  }

  onSubmit() {
    
    this.addressForm.get('nombresProponentes').setValue( null );
    this.procesoSeleccion.cantidadProponentesInvitados = this.addressForm.get('cuantosProponentes').value;
    console.log(this.procesoSeleccion);
    this.guardar.emit(null);
  }

  onSubmitNuevoProponente(){
    this.addressForm.get('nombresProponentes').setValue( null );

    this.guardar.emit(null);
  }

}
