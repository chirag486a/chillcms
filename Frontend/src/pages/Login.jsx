export default function Login() {
  return (
    <div className="register h-full w-full flex items-center justify-center">
      <form action="" className="shadow-2xl bg-primary-content py-16 px-24 rounded-xl">
        <div className="prose">
          <h2 className="text-center">Login</h2>
          <div className="form-control gap-4 w-80">
            <label className="form-control w-full max-w-xs">
              <div className="label">
                <span className="label-text">Email</span>
              </div>
              <input
                type="text"
                placeholder="John@gmail.com"
                className="input input-bordered w-full max-w-xs"
              />
            </label>
            <label htmlFor="" className="form-control w-full max-w-xs">
              <div className="label">
                <span className="label-text">Password</span>
              </div>
              <input
                type="password"
                name=""
                id=""
                placeholder="Password"
                className="input input-bordered w-full max-w-xs"
              />
            </label>
            <div className="form-control gap-3 items-center justify-center my-4">
              <button type="submit" className="btn btn-primary font-thin btn-base-100 w-full">Login</button>
              <a>Create a new account</a>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
}
