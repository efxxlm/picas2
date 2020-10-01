import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ProcesoSeleccion, ProcesoSeleccionCotizacion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-form-estudio-de-mercado',
  templateUrl: './form-estudio-de-mercado.component.html',
  styleUrls: ['./form-estudio-de-mercado.component.scss']
})
export class FormEstudioDeMercadoComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Output() guardar: EventEmitter<any> = new EventEmitter();

  addressForm: FormGroup = this.fb.group({});

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

  createFormulario() {
    return this.fb.group({
      cuantasCotizaciones: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(2)])
      ],
      cotizaciones: this.fb.array([])
    });
  }

  constructor(private fb: FormBuilder,public dialog: MatDialog,) { }
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
        }
      } else if (Formcotizaciones.cuantasCotizaciones <= this.cotizaciones.length && Formcotizaciones.cuantasCotizaciones >= 0) {
        //valido si tiene algo
        let bitVacio=false;
        this.cotizaciones.value.forEach(element => {
          console.log(element);
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
        if(result)
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
      url: [null, Validators.required]
    });
  }

  borrarArray(borrarForm: any, i: number) {    
    borrarForm.removeAt(i);
    //ajusto el contador
    this.addressForm.get('cuantasCotizaciones').setValue(borrarForm.length);    
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  onSubmit() {

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
      };
      this.procesoSeleccion.procesoSeleccionCotizacion.push(cotizacion);
    });

    this.procesoSeleccion.cantidadCotizaciones = listaCotizaciones.length;

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

        listaCotizaciones.push(control);
    });
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
}
