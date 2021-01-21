import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { min } from 'rxjs/operators';
import { EstadosProcesoSeleccion, ProcesoSeleccion, ProcesoSeleccionProponente, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
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
  @Input() editar: boolean;
  @Output() guardar: EventEmitter<any> = new EventEmitter();
  listaProponentes: ProcesoSeleccionProponente[] = [];
  estadosProcesoSeleccion = EstadosProcesoSeleccion;

  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ],
    url: [null, Validators.compose([
      Validators.required])
    ]
  });

  constructor(
    private fb: FormBuilder,
    private procesoSeleccionService: ProcesoSeleccionService,
    private dialog: MatDialog,
    private router: Router,
  ) {

  }


  ngOnInit() {
    return new Promise(resolve => {
      resolve();
    });
  }

  cargarRegistro() {
    this.ngOnInit().then(() => {
      console.log(this.procesoSeleccion.procesoSeleccionProponente.length);
      this.addressForm.get('cuantosProponentes').setValue(1);
      if(this.procesoSeleccion.procesoSeleccionProponente.length>0)
      {
        this.addressForm.get('cuantosProponentes').setValue(this.procesoSeleccion.procesoSeleccionProponente.length);
      }      
      this.addressForm.get('url').setValue(this.procesoSeleccion.urlSoporteProponentesSeleccionados);
    });
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  // no sirvio, ni con este ciclo ni con .include
  validateSel(numeroid: string) {
    
    let retorno= this.valida(numeroid);
    //console.log("valido "+numeroid);
    //console.log(retorno);
    return retorno;
    
  }
  valida(numeroid:string)
  {
    let ret=false;
    this.procesoSeleccion.listaContratistas.forEach(element => {
      if (element.nombre == numeroid) {
        //console.log("valido2 "+element.nombre);
        ret= true;
      }
    });
    return ret;
  }

  onSaveContractors() {

    const proceso: ProcesoSeleccion = {
      numeroProceso: this.procesoSeleccion.numeroProceso,
      procesoSeleccionProponente: this.listaProponentes,
      urlSoporteProponentesSeleccionados: this.addressForm.get('url').value
    };

    // console.log(this.listaProponentes);


    this.procesoSeleccionService.createContractorsFromProponent(proceso)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if (respuesta.code === '200') {
          this.router.navigate(['/seleccion/invitacionCerrada', this.procesoSeleccion.procesoSeleccionId]);
        }
      });

    // this.procesoSeleccion.procesoSeleccionId = this.addressForm.get('procesoSeleccionId').value,
    // this.procesoSeleccion.evaluacionDescripcion = this.addressForm.get('descricion').value,
    // this.procesoSeleccion.urlSoporteEvaluacion = this.addressForm.get('url').value,

    // console.log(procesoS);
    // this.guardar.emit(null);
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      // this.router.navigate(["/seleccion/invitacionCerrada", id]);
      setTimeout(() => {
        location.reload();
      }, 1000);
    });
  }



  onSubmit() {

  }

  changeSeleccion(check, elemento) {

    const cantidad = this.addressForm.get('cuantosProponentes').value ? this.addressForm.get('cuantosProponentes').value : '';

    if (check.checked) {
      if (cantidad >= (this.listaProponentes.length + 1)) {
        this.listaProponentes.push(elemento);
      } else {
        const c: any = document.getElementById(check.id);
        // console.log('check', c );
        c.checked = false;
      }
    }
    else {
      const posicion = this.listaProponentes.indexOf(elemento);
      this.listaProponentes.splice(posicion, 1);

    }
    // console.log( cantidad, check, elemento, this.listaProponentes );
  }

}
