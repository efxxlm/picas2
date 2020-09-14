import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { min } from 'rxjs/operators';
import { ProcesoSeleccion, ProcesoSeleccionProponente, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { $ } from 'protractor';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-seleccion-proponente-a-invitar',
  templateUrl: './form-seleccion-proponente-a-invitar.component.html',
  styleUrls: ['./form-seleccion-proponente-a-invitar.component.scss']
})
export class FormSeleccionProponenteAInvitarComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 
  listaProponentes: ProcesoSeleccionProponente[] = [];

  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ],
    url: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ]
  });

  constructor(
              private fb: FormBuilder,
              private procesoSeleccionService: ProcesoSeleccionService,
              private dialog: MatDialog,    
              private router: Router,

             )
  {

  }


  ngOnInit(): void {
    
  }

  onSaveContractors() {

    let proceso: ProcesoSeleccion = {
      numeroProceso: this.procesoSeleccion.numeroProceso,
      procesoSeleccionProponente: this.listaProponentes,

    }

    console.log(this.listaProponentes);


    this.procesoSeleccionService.createContractorsFromProponent( proceso )
      .subscribe( respuesta => {
        this.openDialog('Proceso Selección', respuesta.message)
        if (respuesta.code == "200")
          this.router.navigate(['/seleccion/invitacionCerrada',this.procesoSeleccion.procesoSeleccionId]);
      })

    // this.procesoSeleccion.procesoSeleccionId = this.addressForm.get('procesoSeleccionId').value,
    // this.procesoSeleccion.evaluacionDescripcion = this.addressForm.get('descricion').value,
    // this.procesoSeleccion.urlSoporteEvaluacion = this.addressForm.get('url').value,
    
    //console.log(procesoS);
    //this.guardar.emit(null);
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  cargarRegistro(){

    // this.addressForm.get('procesoSeleccionId').setValue( this.procesoSeleccion.procesoSeleccionId );
    // this.addressForm.get('descricion').setValue( this.procesoSeleccion.evaluacionDescripcion );
    // this.addressForm.get('url').setValue( this.procesoSeleccion.urlSoporteEvaluacion );

  }

  onSubmit(){

  }

  changeSeleccion( check, elemento ){

    let cantidad = this.addressForm.get('cuantosProponentes').value ? this.addressForm.get('cuantosProponentes').value : 0

    if (check.checked){
      if ( cantidad >= (this.listaProponentes.length + 1) ){
        this.listaProponentes.push( elemento );
      }else{
        let c: any = document.getElementById( check.id );
        //console.log('check', c );    
        c.checked = false;
      }
    }
    else
    {
      let posicion = this.listaProponentes.indexOf( elemento );
      this.listaProponentes.splice( posicion, 1 );
      
    }
    //console.log( cantidad, check, elemento, this.listaProponentes );
  }

}
