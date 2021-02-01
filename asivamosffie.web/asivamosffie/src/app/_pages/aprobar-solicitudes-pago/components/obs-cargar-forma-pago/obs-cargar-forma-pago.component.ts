import { Dominio, CommonService } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-obs-cargar-forma-pago',
  templateUrl: './obs-cargar-forma-pago.component.html',
  styleUrls: ['./obs-cargar-forma-pago.component.scss']
})
export class ObsCargarFormaPagoComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    addressForm: FormGroup;
    solicitudPagoCargarFormaPago: any;
    formaPagoArray: Dominio[] = [];
    editorStyle = {
      height: '45px',
      overflow: 'auto'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
    {
        this.addressForm = this.crearFormulario();
        this.commonSvc.formasDePago()
            .subscribe( response => this.formaPagoArray = response );
    }

    ngOnInit(): void {
        if ( this.solicitudPago !== undefined ) {
            this.solicitudPagoCargarFormaPago = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }
    }

    crearFormulario() {
      return this.fb.group({
        tieneObservaciones: [null, Validators.required],
        observaciones:[null, Validators.required],
      })
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    getFormaPago( formaPagoCodigo: string ) {
        if ( this.formaPagoArray.length > 0 ) {
            const forma = this.formaPagoArray.filter( forma => forma.codigo === formaPagoCodigo );
            return forma[0].nombre;
        }
    }

    onSubmit() {
      console.log(this.addressForm.value);
    }

}
