import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-verificar-balance-gtlc',
  templateUrl: './verificar-balance-gtlc.component.html',
  styleUrls: ['./verificar-balance-gtlc.component.scss']
})
export class VerificarBalanceGtlcComponent implements OnInit {
  idBalance: any;
  addressForm = this.fb.group({});
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
  estaEditando = false;
  constructor(private fb: FormBuilder, private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.activatedRoute.params.subscribe(param => {
      console.log(param.id);
      this.idBalance = param.id;
    });
  }
  crearFormulario() {
    return this.fb.group({
      tieneObservaciones: [null, Validators.required],
      observaciones:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }
  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    console.log(this.addressForm.value);
  }
  irRecursosComprometidos(id){
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/recursosComprometidos', id]);
  }
  verEjecucionFinanciera(id){
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/ejecucionFinanciera', id]);
  }
  verTrasladoRecursos(id){
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/trasladoRecursos', id]);
  }
}
