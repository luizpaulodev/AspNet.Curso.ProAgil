import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_models/User';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registerForm: FormGroup;
  user: User;

  constructor(
    public fb: FormBuilder,
    private toastr: ToastrService,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.validation();
  }

  validation() {
    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      passwords: this.fb.group({
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', Validators.required]
      }, { validator: this.compararSenhas })
    });
  }

  compararSenhas(fb: FormGroup) {
    const confirmSenha = fb.get('confirmPassword');

    if (confirmSenha.errors === null || 'mismatch' in confirmSenha.errors) {
      if (fb.get('password').value === confirmSenha.value) {
        confirmSenha.setErrors(null);
      } else {
        confirmSenha.setErrors({mismatch: true});
      }
    }
  }

  cadastrarUsuario() {
    if (this.registerForm.valid) {
      this.user = Object.assign({ password: this.registerForm.get('passwords.password').value }, this.registerForm.value);

      this.authService.register(this.user).subscribe(
        () => {
          this.router.navigate(['/user/login']);
          this.toastr.success('Cadastrado realizado!');
        }, error => {
          const erro = error.error;

          erro.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                  this.toastr.error('Cadastrado duplicado!');
                  break;

              default:
                  this.toastr.error(`Este nome de usuário não está disponivel! ${element.code}`);
                  break;
            }
          });
        }
      );
    }
  }

}
