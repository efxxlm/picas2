import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ProcesoSeleccion, ProcesoSeleccionCotizacion, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-form-estudio-de-mercado',
  templateUrl: './form-estudio-de-mercado.component.html',
  styleUrls: ['./form-estudio-de-mercado.component.scss']
})
export class FormEstudioDeMercadoComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Input() editar:boolean;
  @Output() guardar: EventEmitter<any> = new EventEmitter();

  addressForm: FormGroup = this.fb.group({});

  estaEditando = false;

  get cotizaciones() {
    return this.addressForm.get('cotizaciones') as FormArray;
  }

  editorStyle = {
    height: '50px',
    color: 'var(--mainColor)'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  noGuardado=true;
  ngOnDestroy(): void {
    if (this.noGuardado===true &&  this.addressForm.dirty) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        // console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSubmit();          
        }           
      });
    }
  };

  createFormulario() {
    return this.fb.group({
      cuantasCotizaciones: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(2)])
      ],
      cotizaciones: this.fb.array([])
    });
  }

  constructor(private fb: FormBuilder,public dialog: MatDialog,private procesoSeleccionService: ProcesoSeleccionService,) { }
  ngOnInit(): void {
    this.addressForm = this.createFormulario();
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  CambioNumeroCotizantes() {
    const Formcotizaciones = this.addressForm.value;
    if(Formcotizaciones.cuantasCotizaciones>0)
    {

      if (Formcotizaciones.cuantasCotizaciones > this.cotizaciones.length && Formcotizaciones.cuantasCotizaciones < 100) {
        while (this.cotizaciones.length < Formcotizaciones.cuantasCotizaciones) {
          this.cotizaciones.push(this.createCotizacion());
          if( this.estaEditando) this.addressForm.markAllAsTouched();
        }
      } else if (Formcotizaciones.cuantasCotizaciones <= this.cotizaciones.length && Formcotizaciones.cuantasCotizaciones >= 0) {
        //valido si tiene algo
        let bitVacio=false;
        this.cotizaciones.value.forEach(element => {
          
          if(element.nombreOrganizacion!=null)
          {
            bitVacio=true;
          }
          if(element.valor!=null)
          {
            bitVacio=true;
          }
          if(element.descripcion!=null)
          {
            bitVacio=true;
          }
          if(element.url!=null)
          {
            bitVacio=true;
          }
        });
        if(bitVacio)
        {

          this.openDialog("","<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>");
          this.addressForm.get("cuantasCotizaciones").setValue(this.cotizaciones.length);
          
        }
        else{
          while (this.cotizaciones.length > Formcotizaciones.cuantasCotizaciones) {
            this.borrarArray(this.cotizaciones, this.cotizaciones.length - 1);
          }
        }        
      }
    }    
  }
  openDialog(modalTitle: string, modalText: string,redirect?:boolean) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        if(result === true)
        {
          //this.router.navigate(["/gestionarFuentes"], {});
        }
      });
    }
  }

  createCotizacion(): FormGroup {
    return this.fb.group({
      procesoSeleccionCotizacionId: [],
      nombreOrganizacion: [null, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(50)])
      ],
      valor: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ],
      descripcion: [null, Validators.required],
      url: [null, Validators.required],
      eliminado:['false']
    });
  }

  borrarArray(borrarForm: any, i: number) {    
    
    //consumo servicio
    console.log(borrarForm.value[i]);
    if(borrarForm.value[i].procesoSeleccionCotizacionId>0)
    {
      this.procesoSeleccionService.deleteProcesoSeleccionCotizacionByID(borrarForm.value[i].procesoSeleccionCotizacionId).subscribe(borrarForm.removeAt(i));
    }
    //ajusto el contador  
    this.addressForm.get('cuantasCotizaciones').setValue(borrarForm.length);    
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    
    if ( texto ){
      saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
      saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');
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
    //console.log(this.procesoSeleccion);return;
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    const listaCotizaciones = this.addressForm.get('cotizaciones') as FormArray;

    this.procesoSeleccion.procesoSeleccionCotizacion = [];

    listaCotizaciones.controls.forEach(control => {
      const cotizacion: ProcesoSeleccionCotizacion = {
        descripcion: control.get('descripcion').value,
        procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
        nombreOrganizacion: control.get('nombreOrganizacion').value,
        procesoSeleccionCotizacionId: control.get('procesoSeleccionCotizacionId').value,
        urlSoporte: control.get('url').value,
        valorCotizacion: control.get('valor').value,
        eliminado:control.get('eliminado').value,
      };
      this.procesoSeleccion.procesoSeleccionCotizacion.push(cotizacion);
    });

    this.procesoSeleccion.cantidadCotizaciones = listaCotizaciones.length;
    this.noGuardado=false;
    this.guardar.emit(null);
  }

  cargarRegistro() {

    const listaCotizaciones = this.addressForm.get('cotizaciones') as FormArray;

    listaCotizaciones.clear();
    this.addressForm.get('cuantasCotizaciones').setValue(this.procesoSeleccion.cantidadCotizaciones);

    this.procesoSeleccion.procesoSeleccionCotizacion.forEach(cotizacion => {
      const control = this.createCotizacion();

      control.get('descripcion').setValue(cotizacion.descripcion),
        control.get('nombreOrganizacion').setValue(cotizacion.nombreOrganizacion),
        control.get('procesoSeleccionCotizacionId').setValue(cotizacion.procesoSeleccionCotizacionId),
        control.get('url').setValue(cotizacion.urlSoporte),
        control.get('valor').setValue(cotizacion.valorCotizacion),
        control.get('eliminado').setValue(cotizacion.eliminado),
        listaCotizaciones.push(control);
    });
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
}
