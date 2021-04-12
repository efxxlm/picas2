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
  sePuedeVer:boolean = false;

  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ],
    url: [null, Validators.compose([
      Validators.required])
    ]
  });
  estaEditando = false;
  cantidadseleccionados = 0;

  constructor(
    private fb: FormBuilder,
    private procesoSeleccionService: ProcesoSeleccionService,
    private dialog: MatDialog,
    private router: Router,
  ) {

  }
  noGuardado=true;
  ngOnDestroy(): void {
    if (this.noGuardado===true &&  this.addressForm.dirty) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSubmit();          
        }           
      });
    }
  };

  ngOnInit() {
    this.estaEditando = this.editar;
    if (this.estaEditando) this.addressForm.markAllAsTouched();
    return new Promise<void>(resolve => {
      resolve();
    });
  }

  cargarRegistro() {
    this.ngOnInit().then(() => {
      console.log(this.procesoSeleccion.procesoSeleccionProponente.length);
      if(this.procesoSeleccion.procesoSeleccionProponente.length>0)
      {
        this.addressForm.get('cuantosProponentes').setValue(this.procesoSeleccion.procesoSeleccionProponente.length);
      }      
      this.addressForm.get('url').setValue(this.procesoSeleccion.urlSoporteProponentesSeleccionados);
    });

    if (
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario
      )
      this.sePuedeVer = true;
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
      cantidadProponentes:this.addressForm.get('cuantosProponentes').value,
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
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.noGuardado=false;
  }

  changeSeleccion(check, elemento) {

    const cantidad = this.addressForm.get('cuantosProponentes').value ? this.addressForm.get('cuantosProponentes').value : '';

    if (check.checked) {
      if (cantidad >= (this.listaProponentes.length + 1)) {
        this.listaProponentes.push(elemento);
      } else {
        // const c: any = document.getElementById(check.id);
        // console.log('check', c );
        check.checked = false;
      }
      this.cantidadseleccionados++;
    }
    else {
      const posicion = this.listaProponentes.indexOf(elemento);
      this.listaProponentes.splice(posicion, 1);
      this.cantidadseleccionados--;
    }
    // console.log( cantidad, check, elemento, this.listaProponentes );
  }

}
